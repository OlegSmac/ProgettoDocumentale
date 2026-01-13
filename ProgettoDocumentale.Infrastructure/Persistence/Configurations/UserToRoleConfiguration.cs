using System.Data.Entity.ModelConfiguration;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class UserToRoleConfiguration : EntityTypeConfiguration<UserToRole>
    {
        public UserToRoleConfiguration()
        {
            HasKey(x => new { x.UserId, x.RoleId });

            HasRequired(x => x.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(true);

            HasRequired(x => x.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(x => x.RoleId)
                .WillCascadeOnDelete(true);
        }
    }
}
