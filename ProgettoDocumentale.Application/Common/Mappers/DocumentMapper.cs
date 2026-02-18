using System;
using System.Linq.Expressions;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;
using ProgettoDocumentale.Application.Requests.Documents.ViewModels;
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
                InstitutionName = document.Institution.Name,
                Username = document.User.UserName,
                TypeId = document.TypeId,
                TypeName = document.Type.Name,
                ProjectId = document.ProjectId,
                ProjectName = document.ProjectId == null ? "-" : document.Project.Name,
                Name = document.Name,
                SavedPath = document.SavedPath,
                UploadDate = document.UploadDate,
                AdditionalInfo = document.AdditionalInfo,
                GroupingDate = document.GroupingDate
            };
        }

        public static Document CreateDocumentRequestDataToDocument(CreateDocumentWithoutFileRequestData req)
        {
            return new Document
            {
                InstitutionId = req.InstitutionId,                
                TypeId = req.TypeId,
                ProjectId = req.ProjectId,
                Name = req.Name,
                SavedPath = req.SavedPath,
                UploadDate = req.UploadDate,
                AdditionalInfo = req.AdditionalInfo,
                GroupingDate = req.GroupingDate
            };
        }

        public static Expression<Func<Document, DocumentDTO>> ToDtoExpr() => document => new DocumentDTO
        {
            Id = document.Id,
            InstitutionId = document.InstitutionId,
            InstitutionName = document.Institution.Name,
            Username = document.User.UserName,
            TypeId = document.TypeId,
            TypeName = document.Type.Name,
            ProjectId = document.ProjectId,
            ProjectName = document.ProjectId == null ? "-" : document.Project.Name,
            Name = document.Name,
            SavedPath = document.SavedPath,
            UploadDate = document.UploadDate,
            AdditionalInfo = document.AdditionalInfo,
            GroupingDate = document.GroupingDate
        };

    }
}
