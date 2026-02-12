using System;
using System.Configuration;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence
{
    public class ProgettoDocContext : DbContext, IProgettoDocContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ProgettoDocContext() : base("name=ProgettoDocumentaleDb")
        { }

        public ProgettoDocContext(IDateTime dateTime, ICurrentUserService currentUserService) : base("name=ProgettoDocumentaleDb")
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }

        public ProgettoDocContext(string connectionString) : base(connectionString)
        { }
        
        public ProgettoDocContext(string connectionString, IDateTime dateTime, ICurrentUserService currentUserService)
            : base(connectionString)
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserToRole> UserToRoles { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentTypeHierarchy> DocumentTypeHierarchies { get; set; }
        public DbSet<Project> Projects { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int userId;

            try
            {
                userId = _currentUserService?.UserId ?? 0;
            }
            catch (TypeLoadException)
            {
                userId = 0;
            }
            if (userId <= 0) userId = 1; //1 is admin id

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {               
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        entry.Property(x => x.Created).IsModified = false;

                        entry.Entity.LastModifiedBy = userId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesBaseAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = 1; //1 is admin id
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        entry.Property(x => x.Created).IsModified = false;

                        entry.Entity.LastModifiedBy = 1;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
