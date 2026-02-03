using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ProgettoDocumentale.Application.Requests.Projects.ViewModels
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectRequestData>
    {
        public CreateProjectValidator() {
            RuleFor(model => model.InstitutionId).NotNull().WithMessage("Institution cannot be null")
                                .NotEqual(-1).WithMessage("Choose institution");

            RuleFor(model => model.Name).NotEmpty().WithMessage("Name cannot be empty")
                                .NotNull().WithMessage("Name cannot be null")
                                .MinimumLength(1).WithMessage("Name must be at least 1 character")
                                .MaximumLength(100).WithMessage("Name must be at most 100 characters");

            RuleFor(model => model.DateFrom).NotEmpty().WithMessage("DateFrom cannot be empty")
                                .NotNull().WithMessage("DateFrom cannot be null");

            RuleFor(model => model.DateTill).NotEmpty().WithMessage("DateTill cannot be empty")
                                .NotNull().WithMessage("DateTill cannot be null")
                                .GreaterThanOrEqualTo(model => model.DateFrom).WithMessage("End date can't be less than start");

            RuleFor(model => model.AdditionalInfo).MaximumLength(500).WithMessage("Additional info must be at most 500 characters");
        }
    }
}
