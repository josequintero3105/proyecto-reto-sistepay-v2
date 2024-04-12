﻿using System.ComponentModel;

namespace Common.Helpers.Exceptions;

public enum CommonExceptionTypes
{
    [Description("Excepción no controlada.")]
    NotControlledException,

    [Description("Ambiente errado.")]
    WrongEnviromentValue,

    [Description("Header {0} es requerido.")]
    HeaderIsRequired,
}