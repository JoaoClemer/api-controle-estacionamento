using ControleDeEstacionamento.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeEstacionamento.Data.Mappings.cs
{
    public class ParkingMap : IEntityTypeConfiguration<Parking>
    {
        public void Configure(EntityTypeBuilder<Parking> builder)
        {
            builder.HasKey(x => x.Id);
            //builder.Property(x => x.Id)
            //    .ValueGeneratedOnAdd();

            builder.Property(x => x.TotalParkingSpots)
                .IsRequired();

            builder.HasOne(x => x.Company)
                .WithMany(x => x.Parkings)
                .HasForeignKey(x=>x.CompanyId);
        }
    }
}
