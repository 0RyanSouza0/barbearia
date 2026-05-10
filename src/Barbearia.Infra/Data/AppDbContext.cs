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
    public DbSet<Administrador> Administradors { get; set; }
    public DbSet<Servicos> Servicos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<ItemPedido> ItensPedidos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>()
        .Property(a => a.Status)
        .HasConversion<string>();

        modelBuilder.Entity<Administrador>()
       .Property(a => a.Role)
       .HasConversion<string>();

        modelBuilder.Entity<Administrador>()
        .HasQueryFilter(a => a.Ativo);

        modelBuilder.Entity<Administrador>()
               .HasIndex(a => a.Email)
               .IsUnique();
        modelBuilder.Entity<Pedido>().HasQueryFilter(p => p.Ativo);
        modelBuilder.Entity<Pedido>()
        .Property(p => p.Status)
        .HasConversion<string>();

        modelBuilder.Entity<Produto>().HasQueryFilter(p => p.Ativo);
        modelBuilder.Entity<Produto>()
        .Property(p => p.Categoria)
        .HasConversion<string>();

        modelBuilder.Entity<Endereco>()
        .Property(e => e.Cep)
        .HasMaxLength(8);

        modelBuilder.Entity<Cliente>().HasQueryFilter(c => c.Ativo);
        modelBuilder.Entity<Cliente>().Property(c => c.Role).HasConversion<string>();
        modelBuilder.Entity<Cliente>()
        .HasIndex(c => c.Email)
        .IsUnique();
    }
}
