using ControleDeEstacionamento.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeEstacionamento.Data.Mappings.cs
{
    public class VehicleMap : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(x => x.Id);
           

            builder.Property(x => x.LicensePlate)
                .IsRequired()
                .HasMaxLength(7);

            builder.Property(x => x.Year)
                .IsRequired()
                .HasMaxLength(4);

            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Color)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.EntryTime)
                .IsRequired();

            builder.Property(x => x.ExitTime);

            builder.HasIndex(x => x.LicensePlate)
                .IsUnique();

            builder.HasOne(x => x.Parking)
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x=> x.ParkingId);
 
         }
    }
}
