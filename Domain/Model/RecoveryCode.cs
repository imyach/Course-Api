using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class RecoveryCode : BaseModel
    {
        public Guid UserId { get; set; }
        public string CodeHash{ get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
        public bool IsUsedEarlier { get; set; }

        public User? User { get; set; }
    }
}
