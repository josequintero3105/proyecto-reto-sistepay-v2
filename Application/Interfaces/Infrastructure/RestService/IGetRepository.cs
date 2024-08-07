﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Commands;

namespace Application.Interfaces.Infrastructure.RestService
{
    public interface IGetRepository
    {
        Task<HttpResponseMessage> GetTAdapter(dynamic request, NameValueCollection _id);
    }
}
