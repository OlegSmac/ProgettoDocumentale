using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Infrastructure.Persistence.Configurations
{
    public class DocumentTypeHierarchyConfiguration : IEntityTypeConfiguration<DocumentTypeHierarchy>
    {
        public void Configure(EntityTypeBuilder<DocumentTypeHierarchy> builder)
        {
            builder.HasKey(x => new { x.MacroId, x.MicroId });

            builder.HasOne(x => x.Macro)
                .WithMany(t => t.MicroTypes)
                .HasForeignKey(x => x.MacroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.MicroId);
            builder.HasOne(x => x.Micro)
                .WithMany(d => d.MacroTypes)
                .HasForeignKey(x => x.MicroId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
