using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Question : BaseModel
    {
        public Guid TestId { get; set; }
        public string Text { get; set; } = string.Empty;

        public Test? Test { get; set; }

        public IEnumerable<AnswersUser>? AnswersUsers { get; set; }
        public IEnumerable<Answer>? Answers { get; set; }
    }
}
