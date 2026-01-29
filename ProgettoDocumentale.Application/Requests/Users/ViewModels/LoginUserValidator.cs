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
        { }
    }
}