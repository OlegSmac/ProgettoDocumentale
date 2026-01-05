using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(32);

            builder.HasIndex(x => x.UserName)
                .IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(254);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.Property(x => x.Surname)
                .HasMaxLength(100);

            builder.Property(x => x.Patronymic)
                .HasMaxLength(100);

            builder.Property(x => x.IsEnabled)
                .IsRequired();

            builder.Property(x => x.InstitutionId)
                .IsRequired();

            builder.HasIndex(x => x.InstitutionId);
            builder.HasOne(x => x.Institution)
                .WithMany(i => i.Users)
                .HasForeignKey(x => x.InstitutionId)
                .OnDelete(DeleteBehavior.Restrict);        
        }
    }
}
