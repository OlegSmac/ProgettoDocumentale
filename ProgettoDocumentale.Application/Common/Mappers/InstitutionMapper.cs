using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProgettoDocumentale.Application.Requests.Institutions.DTOs;
using ProgettoDocumentale.Application.Requests.Institutions.ViewModels;
using ProgettoDocumentale.Application.Requests.Users.DTOs;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Conmmon.Mappers
{
    public static class InstitutionMapper
    {
        public static InstitutionDTO InstitutionToInstitutionDTO(Institution institution)
        {
            return new InstitutionDTO
            {
                Id = institution.Id,
                InstCode = institution.InstCode,
                Name = institution.Name,
                AdditionalInfo = institution.AdditionalInfo                
            };
        }

        public static Institution CreateInstitutionRequestDataToInstitution(CreateInstitutionRequestData req)
        {
            return new Institution
            {
                InstCode = req.InstCode,
                Name = req.Name,
                AdditionalInfo = req.AdditionalInfo
            };
        }

        public static Expression<Func<Institution, InstitutionDTO>> ToDtoExpr() => institution => new InstitutionDTO
        {
            Id = institution.Id,
            InstCode = institution.InstCode,
            Name = institution.Name,
            AdditionalInfo = institution.AdditionalInfo
        };
    }
}
