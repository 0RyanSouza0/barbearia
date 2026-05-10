using Barbearia.Application.Interfaces.Repository;
using Barbearia.Infra.Repository;

namespace Barbearia.Api.Dependencies.Repository;

public static class RepositoryDI
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IServicosRepository, ServicosRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IBarbeiroRepository, BarbeiroRepository>();
        services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
        return services;
    }
}
