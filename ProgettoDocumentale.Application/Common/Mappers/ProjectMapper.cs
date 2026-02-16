using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProgettoDocumentale.Application.Requests.Projects.DTOs;
using ProgettoDocumentale.Application.Requests.Projects.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Common.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDTO ProjectToProjectDTO(Project project)
        {
            return new ProjectDTO
            {
                Id = project.Id,
                InstitutionId = project.InstitutionId,
                InstitutionName = project.Institution.Name,
                UserId = project.UserId,
                Username = project.User.UserName,
                Name = project.Name,
                DateFrom = project.DateFrom,
                DateTill = project.DateTill,
                AdditionalInfo = project.AdditionalInfo,
                IsActive = project.IsActive
            };
        }

        public static Project CreateProjectRequestDataToProject(CreateProjectRequestData req)
        {
            return new Project
            {
                InstitutionId = req.InstitutionId,
                Name = req.Name,
                DateFrom = req.DateFrom,
                DateTill = req.DateTill,
                AdditionalInfo = req.AdditionalInfo,
                IsActive = req.IsActive
            };
        }

        public static Expression<Func<Project, ProjectDTO>> ToDtoExpr() => project => new ProjectDTO
        {
            Id = project.Id,
            InstitutionId = project.InstitutionId,
            InstitutionName = project.Institution.Name,
            UserId = project.UserId,
            Username = project.User.UserName,
            Name = project.Name,
            DateFrom = project.DateFrom,
            DateTill = project.DateTill,
            AdditionalInfo = project.AdditionalInfo,
            IsActive = project.IsActive
        };

    }
}
