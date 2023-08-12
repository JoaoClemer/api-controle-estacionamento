using ControleDeEstacionamento.Data.Mappings.cs;
using ControleDeEstacionamento.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstacionamento.Data
{
    public class ParkingDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DataSource=parking.db;Cache=Shared");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new VehicleMap());
            modelBuilder.ApplyConfiguration(new ParkingMap());
        }

    }

}
