using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using MediatR;
using ProgettoDocumentale.Application.DTOs.User;

namespace ProgettoDocumentale.Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        //private readonly IValidator<AddUserDto> _userValidator;
        //private readonly IValidator<UserDto> _updateUserValidator;

        public AdminController(IMediator mediator) // IValidator<AddUserDto> userValidator, IValidator<UserDto> updateUserValidator
        {
            _mediator = mediator;
            //_userValidator = userValidator;
            //_updateUserValidator = updateUserValidator;
        }

        
    }
}