using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Answer : BaseModel
    {
        public Guid IdQuestion { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public Question Question { get; set; }

        public IEnumerable<AnswersUser> AnswersUsers { get; set; }
    }
}
