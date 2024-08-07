﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ApiEntities.Input
{
    public class Sandbox
    {
        /// <summary>
        /// IsActive
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string? Status { get; set; }
    }

    public class SandboxInactive
    {
        /// <summary>
        /// Status
        /// </summary>
        public string? Status { get; set; }
    }
}
