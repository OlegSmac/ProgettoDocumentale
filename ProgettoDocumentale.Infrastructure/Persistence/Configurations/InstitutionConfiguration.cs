using System.Data.Entity.ModelConfiguration;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class InstitutionConfiguration : EntityTypeConfiguration<Institution>
    {
        public InstitutionConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.InstCode)
                .IsRequired()
                .HasMaxLength(5);

            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            Property(x => x.AdditionalInfo)
                .HasMaxLength(1000);
        }
    }
}
