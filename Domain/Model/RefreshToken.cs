using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class RefreshToken : BaseModel
    { 
        public Guid UserId { get; set; }
        public Guid ResreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public DateTime CreatedAt { get; set; }


        public User? User { get; set; }

    }
}
