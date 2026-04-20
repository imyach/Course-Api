using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Auth
{
    public class CodeRecoveryDto
    {
        public int Code { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
