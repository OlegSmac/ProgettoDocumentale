using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ProgettoDocumentale.Application.Requests.Documents.ViewModels
{
    public class CreateDocumentValidator : AbstractValidator<CreateDocumentRequestData>
    {
        public CreateDocumentValidator() {
            RuleFor(model => model.InstitutionId).NotNull().WithMessage("Institution cannot be null")
                                .NotEqual(-1).WithMessage("Choose institution");

            RuleFor(model => model.Username).NotNull().WithMessage("User cannot be null")
                                .NotNull().WithMessage("Choose user");

            RuleFor(model => model.MacroTypeId).NotNull().WithMessage("Type cannot be null")
                                .NotEqual(-1).WithMessage("Choose type");

            RuleFor(model => model.TypeId).NotNull().WithMessage("Type cannot be null")
                                .NotEqual(-1).WithMessage("Choose type");            

            RuleFor(model => model.Name).NotEmpty().WithMessage("Name cannot be empty")
                                .NotNull().WithMessage("Name cannot be null")
                                .MinimumLength(1).WithMessage("Name must be at least 1 character")
                                .MaximumLength(100).WithMessage("Name must be at most 100 characters");            

            RuleFor(model => model.UploadDate).NotEmpty().WithMessage("Upload date cannot be empty")
                                .NotNull().WithMessage("Upload date cannot be null");

            RuleFor(model => model.AdditionalInfo).MaximumLength(500).WithMessage("Additional info must be at most 500 characters");

            RuleFor(model => model.GroupingDate).NotEmpty().WithMessage("Upload date cannot be empty")
                                .NotNull().WithMessage("Upload date cannot be null");
        }
    }
}
