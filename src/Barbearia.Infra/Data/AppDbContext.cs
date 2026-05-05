using Barbearia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Barbearia.Infra.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Barbeiro> Barbeiros { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedidos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>()
        .Property(a => a.Status)
        .HasConversion<string>();

        modelBuilder.Entity<Pedido>()
        .Property(p => p.Status)
        .HasConversion<string>();

        modelBuilder.Entity<Produto>()
        .Property(p => p.Categoria)
        .HasConversion<string>();

        modelBuilder.Entity<Endereco>()
        .Property(e => e.Cep)
        .HasMaxLength(8);

        modelBuilder.Entity<Cliente>()
        .HasIndex(c => c.Email)
        .IsUnique();
    }
}
