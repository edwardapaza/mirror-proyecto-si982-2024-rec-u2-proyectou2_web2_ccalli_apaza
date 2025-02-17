using Microsoft.EntityFrameworkCore;
using Animalia.Models;

namespace Animalia.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<vw_ConsultasNotificaciones> vw_ConsultasNotificaciones { get; set; }
        public DbSet<Diagnostico> Diagnosticos { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().ToTable("usuarios");
            modelBuilder.Entity<Cliente>().ToTable("clientes");
            modelBuilder.Entity<Mascota>().ToTable("mascotas");
            modelBuilder.Entity<Consulta>().ToTable("consultas");
            modelBuilder.Entity<Veterinario>().ToTable("veterinarios");
            modelBuilder.Entity<vw_ConsultasNotificaciones>().HasNoKey().ToView("vw_ConsultasNotificaciones");


            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Mascota)
                .WithMany()
                .HasForeignKey(c => c.IdMascota)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Diagnostico)
                .WithOne(d => d.Consulta)
                .HasForeignKey<Diagnostico>(d => d.IdConsulta)
                .IsRequired(true); // or .IsRequired(false) if Diagnostico is optional for Consulta

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Veterinario)
                .WithMany()
                .HasForeignKey(c => c.IdVeterinario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18, 2)");
            // REMOVE THIS CONFLICTING CONFIGURATION ENTIRELY:
            // modelBuilder.Entity<Diagnostico>()
            //     .HasOne(d => d.Consulta)
            //     .WithMany()
            //     .HasForeignKey(d => d.IdConsulta)
            //     .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
