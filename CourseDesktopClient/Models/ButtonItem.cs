using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CourseDesktopClient.Models
{
    public class ButtonItem
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public bool IsEllipsis { get; set; }
    }
}
