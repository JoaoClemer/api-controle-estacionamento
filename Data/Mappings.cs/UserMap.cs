using ControleDeEstacionamento.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeEstacionamento.Data.Mappings.cs
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.Role)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasOne(x => x.Company)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.CompanyId);

        }
    }
}
