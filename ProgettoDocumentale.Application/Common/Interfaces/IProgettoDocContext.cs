using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Common.Interfaces
{
    public interface IProgettoDocContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserToRole> UserToRoles { get; set; }
        DbSet<Institution> Institutions { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<DocumentType> DocumentTypes { get; set; }
        DbSet<Project> Projects { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
