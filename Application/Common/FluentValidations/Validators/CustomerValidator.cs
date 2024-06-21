﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Entries;
using Common.Helpers.Exceptions;
using FluentValidation;

namespace Application.Common.FluentValidations.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerInput>
    {
        public CustomerValidator() 
        {
            RuleFor(c => c.Name)
                .NotNull()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerNameCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerNameCannotBeEmpty))
                .NotEmpty()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerNameCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerNameCannotBeEmpty))
                .MaximumLength(50)
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerNameCannotBeVeryLong).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerNameCannotBeVeryLong))
                .Matches("^[a-zA-Z0-9 ]+$")
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.NotAllowSpecialCharacters).ToString())
                .WithMessage(nameof(GateWayBusinessException.NotAllowSpecialCharacters));
            RuleFor(c => c.DocumentType)
                .NotNull()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerDocumentTypeCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerDocumentTypeCannotBeEmpty))
                .NotEmpty()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerDocumentTypeCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerDocumentTypeCannotBeEmpty))
                .Matches("^[a-zA-Z0-9 ]+$")
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.NotAllowSpecialCharacters).ToString())
                .WithMessage(nameof(GateWayBusinessException.NotAllowSpecialCharacters));
            RuleFor(c => c.Document)
                .NotNull()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerDocumentCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerDocumentCannotBeEmpty))
                .NotEmpty()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerDocumentCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerDocumentCannotBeEmpty))
                .MaximumLength(50)
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerDocumentCannotBeVeryLong).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerDocumentCannotBeVeryLong))
                .Matches("^[a-zA-Z0-9 ]+$")
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.NotAllowSpecialCharacters).ToString())
                .WithMessage(nameof(GateWayBusinessException.NotAllowSpecialCharacters));
            RuleFor(c => c.Email)
                .NotNull()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerEmailCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerEmailCannotBeEmpty))
                .NotEmpty()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerEmailCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerEmailCannotBeEmpty));
            RuleFor(c => c.Phone)
                .NotNull()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerPhoneCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerPhoneCannotBeEmpty))
                .NotEmpty()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerPhoneCannotBeEmpty).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerPhoneCannotBeEmpty))
                .MaximumLength(50)
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerPhoneCannotBeVeryLong).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerPhoneCannotBeVeryLong))
                .Matches("^[a-zA-Z0-9 ]+$")
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.NotAllowSpecialCharacters).ToString())
                .WithMessage(nameof(GateWayBusinessException.NotAllowSpecialCharacters));
        }
    }
}
