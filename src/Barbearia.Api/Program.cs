using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Barbearia.Application.Validators.Cliente;
using FluentValidation;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.OpenApi.MicrosoftExtensions;
using Barbearia.Api.Dependencies.Services;
using Barbearia.Api.Dependencies.Repository;
using Barbearia.Domain.Enuns;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Barbearia.Application.Interfaces.Auth;
using Barbearia.Infra.Auth;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddServices();
builder.Services.AddRepository();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Configura o Enum 
builder.Services.AddControllers()

.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ClienteRequestValidator>();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Role.Admin.ToString(), policy => policy.RequireRole("Admin"))
    .AddPolicy(Role.Cliente.ToString(), policy => policy.RequireRole("Cliente"));

builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
// ✅ Com security scheme
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((doc, context, _) =>
    {
        doc.Components ??= new OpenApiComponents();
        doc.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Insira o token JWT"
            }
        };
        return Task.CompletedTask;
    });
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
 {
     options.WithTitle("Barbearia API")
            .WithTheme(ScalarTheme.Moon)
            .WithPreferredScheme("Bearer")
            .WithHttpBearerAuthentication(bearer =>
            {
                bearer.Token = "";
            });
 });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
