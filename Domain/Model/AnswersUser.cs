using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class AnswersUser : BaseModel
    {
        public Guid IdUser { get; set; }
        public Guid IdQuestion { get; set; }
        public Guid IdAnswer { get; set; }

        public Question Question { get; set; }
        public Answer Answer { get; set; }
        public User User { get; set; }
    }
}
