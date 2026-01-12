using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using ProgettoDocumentale.Presentation.Models.Account;

namespace ProgettoDocumentale.Presentation.Contracts.Validations
{
    public class LoginUserCommandValidator : AbstractValidator<LoginModel>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(model => model.UserName).NotEmpty().WithMessage("Username cannot be empty")
                                .NotNull().WithMessage("Username cannot be null")
                                .MinimumLength(4).WithMessage("Username must be at least 4 characters")
                                .MaximumLength(32).WithMessage("Username must be at most 32 characters");

            RuleFor(model => model.Password).NotEmpty().WithMessage("Password cannot be empty")
                                .NotNull().WithMessage("Password cannot be null")
                                .MinimumLength(8).WithMessage("Password must be at least 8 characters");
        }
    }
}