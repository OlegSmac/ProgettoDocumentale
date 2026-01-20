using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace ProgettoDocumentale.Application.Requests.Institutions.ViewModels
{
    public class UpdateInstitutionValidator : AbstractValidator<UpdateInstitutionRequestData>
    {
        public UpdateInstitutionValidator() 
        {
            RuleFor(model => model.Id).NotNull().WithMessage("Id cannot be null");

            RuleFor(model => model.InstCode).NotEmpty().WithMessage("InstCode cannot be empty")
                                .NotNull().WithMessage("InstCode cannot be null")
                                .MinimumLength(2).WithMessage("Username must be at least 2 characters")
                                .MaximumLength(5).WithMessage("Username must be at most 5 characters");

            RuleFor(model => model.Name).NotEmpty().WithMessage("Name cannot be empty")
                                .NotNull().WithMessage("Name cannot be null")
                                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                                .MaximumLength(255).WithMessage("Name must be at most 255 characters");

            RuleFor(model => model.AdditionalInfo).NotEmpty().WithMessage("AdditionalInfo cannot be empty")
                                .NotNull().WithMessage("AdditionalInfo cannot be null")
                                .MinimumLength(5).WithMessage("AdditionalInfo must be at least 5 characters")
                                .MaximumLength(500).WithMessage("AdditionalInfo must be at most 500 characters");
        }
    }
}