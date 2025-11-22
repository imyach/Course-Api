using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Base
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
