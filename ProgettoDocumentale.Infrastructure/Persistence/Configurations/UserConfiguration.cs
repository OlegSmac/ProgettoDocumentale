using System.Data.Entity.ModelConfiguration;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(32);

            Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(254);

            Property(x => x.Name)
                .HasMaxLength(100);

            Property(x => x.Surname)
                .HasMaxLength(100);

            Property(x => x.Patronymic)
                .HasMaxLength(100);

            Property(x => x.IsEnabled)
                .IsRequired();

            Property(x => x.InstitutionId)
                .IsRequired();

            HasRequired(x => x.Institution)
                .WithMany(i => i.Users)
                .HasForeignKey(x => x.InstitutionId)
                .WillCascadeOnDelete(false);
        }
    }
}
