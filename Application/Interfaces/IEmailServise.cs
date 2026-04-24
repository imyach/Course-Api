using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IEmailServise
    {
        public  Task SendMessage(string message, string miniDescription, string userEmail);
    }
}
