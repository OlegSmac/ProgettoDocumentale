using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;
using ProgettoDocumentale.Application.Requests.Projects.DTOs;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Common.Mappers
{
    public static class DocumentMapper
    {
        public static DocumentDTO DocumentToDocumentDTO(Document document)
        {
            return new DocumentDTO
            {
                Id = document.Id,
                InstitutionId = document.InstitutionId,
                UserId = document.UserId,
                TypeId = document.TypeId,
                ProjectId = document.ProjectId,
                Name = document.Name,
                SavedPath = document.SavedPath,
                UploadDate = document.UploadDate,
                AdditionalInfo = document.AdditionalInfo,
                GroupingDate = document.GroupingDate
            };
        }

        public static Expression<Func<Document, DocumentDTO>> ToDtoExpr() => document => new DocumentDTO
        {
            Id = document.Id,
            InstitutionId = document.InstitutionId,
            UserId = document.UserId,
            TypeId = document.TypeId,
            ProjectId = document.ProjectId,
            Name = document.Name,
            SavedPath = document.SavedPath,
            UploadDate = document.UploadDate,
            AdditionalInfo = document.AdditionalInfo,
            GroupingDate = document.GroupingDate
        };
    }
}
