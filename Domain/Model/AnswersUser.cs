using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class AnswersUser : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid AnswerId { get; set; }

        public Answer? Answer { get; set; }
        public User? User { get; set; }
    }
}
