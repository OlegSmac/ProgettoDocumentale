using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace ProgettoDocumentale.Application.Requests.Users.ViewModels
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordData>
    {
        public ResetPasswordValidator() 
        {
            RuleFor(model => model.UserName).NotEmpty().WithMessage("Username cannot be empty")
                                .NotNull().WithMessage("Username cannot be null")
                                .MinimumLength(4).WithMessage("Username must be at least 4 characters")
                                .MaximumLength(32).WithMessage("Username must be at most 32 characters");

            RuleFor(model => model.OldPassword).NotEmpty().WithMessage("OldPassword cannot be empty")
                                .NotNull().WithMessage("OldPassword cannot be null")
                                .MinimumLength(4).WithMessage("OldPassword must be at least 4 characters")
                                .MaximumLength(50).WithMessage("OldPassword must be at most 50 characters");

            RuleFor(model => model.NewPassword).NotEmpty().WithMessage("NewPassword cannot be empty")
                                .NotNull().WithMessage("NewPassword cannot be null")
                                .MinimumLength(4).WithMessage("NewPassword must be at least 4 characters")
                                .MaximumLength(50).WithMessage("NewPassword must be at most 50 characters");
        } 
    }
}