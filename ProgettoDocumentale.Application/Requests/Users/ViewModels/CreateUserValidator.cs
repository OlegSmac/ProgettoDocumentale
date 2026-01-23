using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace ProgettoDocumentale.Application.Requests.Users.ViewModels
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequestData>
    {
        public CreateUserValidator() 
        {
            RuleFor(model => model.InstitutionId).NotNull().WithMessage("Institution cannot be null")
                                .NotEqual(-1).WithMessage("Choose institution");

            RuleFor(model => model.UserName).NotEmpty().WithMessage("Username cannot be empty")
                                .NotNull().WithMessage("Username cannot be null")
                                .MinimumLength(4).WithMessage("Username must be at least 4 characters")
                                .MaximumLength(32).WithMessage("Username must be at most 32 characters");

            RuleFor(model => model.Password).NotEmpty().WithMessage("Password cannot be empty")
                                .NotNull().WithMessage("Password cannot be null")
                                .MinimumLength(4).WithMessage("Password must be at least 4 characters")
                                .MaximumLength(50).WithMessage("Password must be at most 50 characters");

            RuleFor(model => model.Email).NotEmpty().WithMessage("Email cannot be empty")
                                .NotNull().WithMessage("Email cannot be null")
                                .MinimumLength(4).WithMessage("Email must be at least 4 characters")
                                .MaximumLength(255).WithMessage("Email must be at most 255 characters");

            RuleFor(model => model.Name).NotEmpty().WithMessage("Name cannot be empty")
                                .NotNull().WithMessage("Name cannot be null")
                                .MinimumLength(1).WithMessage("Name must be at least 1 character")
                                .MaximumLength(100).WithMessage("Name must be at most 100 characters");

            RuleFor(model => model.Surname).NotEmpty().WithMessage("Surname cannot be empty")
                                .NotNull().WithMessage("Surname cannot be null")
                                .MinimumLength(1).WithMessage("Surname must be at least 1 character")
                                .MaximumLength(100).WithMessage("Surname must be at most 100 characters");

            RuleFor(model => model.Patronymic).NotEmpty().WithMessage("Patronymic cannot be empty")
                                .NotNull().WithMessage("Patronymic cannot be null")
                                .MinimumLength(1).WithMessage("Patronymic must be at least 1 character")
                                .MaximumLength(100).WithMessage("Patronymic must be at most 100 characters");

            RuleFor(model => model.Roles).NotEmpty().WithMessage("At least 1 role should be selected");
        }
    }
}