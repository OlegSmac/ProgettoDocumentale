using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class DocumentTypeHierarchyConfiguration : EntityTypeConfiguration<DocumentTypeHierarchy>
    {
        public DocumentTypeHierarchyConfiguration()
        {
            HasKey(x => new { x.MacroId, x.MicroId });

            HasRequired(x => x.Macro)
                .WithMany(t => t.MicroTypes)
                .HasForeignKey(x => x.MacroId)
                .WillCascadeOnDelete(false);
            
            HasRequired(x => x.Micro)
                .WithMany(d => d.MacroTypes)
                .HasForeignKey(x => x.MicroId)
                .WillCascadeOnDelete(false);
        }
    }
}
