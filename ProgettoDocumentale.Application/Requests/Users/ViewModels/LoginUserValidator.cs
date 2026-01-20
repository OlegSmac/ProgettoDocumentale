using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace ProgettoDocumentale.Application.Requests.Users.ViewModels
{
    public class LoginUserValidator : AbstractValidator<LoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(model => model.UserName).NotEmpty().WithMessage("Username cannot be empty")
                                .NotNull().WithMessage("Username cannot be null")
                                .MinimumLength(4).WithMessage("Username must be at least 4 characters")
                                .MaximumLength(32).WithMessage("Username must be at most 32 characters");

            RuleFor(model => model.Password).NotEmpty().WithMessage("Password cannot be empty")
                                .NotNull().WithMessage("Password cannot be null")
                                .MinimumLength(4).WithMessage("Password must be at least 4 characters")
                                .MaximumLength(50).WithMessage("Password must be at most 50 characters");
        }
    }
}