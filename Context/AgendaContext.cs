using API.Dtos;
using Microsoft.EntityFrameworkCore;
using API.Entities;
namespace API.Context;

public class AgendaContext: DbContext
{
    public DbSet<User> Users { get; set; } // Informando que é uma tabela no banco de dados
    public DbSet<Service> Services { get; set; }
    
    public DbSet<Appointments> Appointments { get; set; }
    
    public DbSet<Availability> Availabilities { get; set; }
    
    // Passando a aconexão do banco para a classe pai DbContext
    public AgendaContext(DbContextOptions<AgendaContext> options): base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointments>()
            .HasOne(s => s.Service)
            .WithMany()
            .HasForeignKey(s => s.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointments>()
            .HasOne(s => s.Client)
            .WithMany()
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        modelBuilder.Entity<Appointments>()
            .HasOne(s => s.Client)
            .WithMany()
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Appointments>()
            .HasOne(s => s.Professional)
            .WithMany()
            .HasForeignKey(s => s.ProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}