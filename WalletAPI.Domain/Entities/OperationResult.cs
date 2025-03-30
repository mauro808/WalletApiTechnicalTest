﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Domain.Entities
{
    public class OperationResult
    {
        public bool Success { get; }
        public string Message { get; }

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
