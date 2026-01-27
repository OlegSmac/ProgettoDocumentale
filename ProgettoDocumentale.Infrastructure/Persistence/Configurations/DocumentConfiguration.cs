using System.Data.Entity.ModelConfiguration;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        public DocumentConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(260);

            Property(x => x.SavedPath)
                .HasMaxLength(3000);

            Property(x => x.AdditionalInfo)
                .HasMaxLength(1000);

            HasRequired(x => x.Institution)
                .WithMany(i => i.Documents)
                .HasForeignKey(x => x.InstitutionId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.User)
                .WithMany(u => u.Documents)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Type)
                .WithMany(t => t.Documents)
                .HasForeignKey(x => x.TypeId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Project)
                .WithMany(t => t.Documents)
                .HasForeignKey(x => x.ProjectId)
                .WillCascadeOnDelete(false);
        }
    }
}
