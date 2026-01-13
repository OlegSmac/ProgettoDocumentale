using System.Data.Entity.ModelConfiguration;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class DocumentTypeConfiguration : EntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Code)
                .HasMaxLength(5);

            Property(x => x.Name)
                .HasMaxLength(255);

            Property(x => x.TypeDscr)
                .HasMaxLength(500);
        }
    }
}
