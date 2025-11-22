using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Matherial : BaseModel
    {
        public Guid IdModule { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }

        public Module Module { get; set; }

        public IEnumerable<Test> Tests { get; set; }
    }
}
