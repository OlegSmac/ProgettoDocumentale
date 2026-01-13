using System.Data.Entity.ModelConfiguration;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            Property(x => x.AdditionalInfo)
                .HasMaxLength(1000);

            HasRequired(x => x.Institution)
                .WithMany(i => i.Projects)
                .HasForeignKey(x => x.InstitutionId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

        }
    }
}
