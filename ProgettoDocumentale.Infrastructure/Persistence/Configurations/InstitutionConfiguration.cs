using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class InstitutionConfiguration
    {
        public void Configure(EntityTypeBuilder<Institution> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.InstCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.HasIndex(x => x.InstCode)
                .IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.AdditionalInfo)
                .HasMaxLength(1000);
        }
    }
}
