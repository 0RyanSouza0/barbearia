using Barbearia.Application.Interfaces.Service;
using Barbearia.Application.Service;

namespace Barbearia.Api.Dependencies.Services;

public static class ServicesDI
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IServicosService, ServicosService>();
        services.AddScoped<IEnderecoService, EnderecoService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IBarbeiroService, BarbeiroService>();
        services.AddScoped<IAdministradorService, AdministradorService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAgendamentoService, AgendamentoService>();
        return services;
    }

}
