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
    public class InvoiceValidator : AbstractValidator<InvoiceInput>
    {
        public InvoiceValidator() 
        {
            RuleFor(i => i.ShoppingCartId)
                .NotEmpty()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.ShoppingCartIdIsNotValid).ToString())
                .WithMessage(nameof(GateWayBusinessException.ShoppingCartIdIsNotValid))
                .MaximumLength(24)
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.ShoppingCartIdIsNotValid).ToString())
                .WithMessage(nameof(GateWayBusinessException.ShoppingCartIdIsNotValid))
                .MinimumLength(24)
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.ShoppingCartIdIsNotValid).ToString())
                .WithMessage(nameof(GateWayBusinessException.ShoppingCartIdIsNotValid))
                .Matches("^[a-zA-Z0-9 ]+$")
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.NotAllowSpecialCharacters).ToString())
                .WithMessage(nameof(GateWayBusinessException.NotAllowSpecialCharacters));
            RuleFor(i => i.CustomerId)
                .NotEmpty()
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerIdIsNotValid).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerIdIsNotValid))
                .MaximumLength(24)
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerIdIsNotValid).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerIdIsNotValid))
                .MinimumLength(24)
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.CustomerIdIsNotValid).ToString())
                .WithMessage(nameof(GateWayBusinessException.CustomerIdIsNotValid))
                .Matches("^[a-zA-Z0-9 ]+$")
                .WithErrorCode(Convert.ToInt32(GateWayBusinessException.NotAllowSpecialCharacters).ToString())
                .WithMessage(nameof(GateWayBusinessException.NotAllowSpecialCharacters));
        }
    }
}
