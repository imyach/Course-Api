using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CreateCoursePanelElementVm(CourseDto courseDto)
    {
        public string Title => courseDto.Title;
        public string? Description => courseDto.Description;
        public Guid Id => courseDto.Id;
        public UserDto User => courseDto.User;
        public DateTime CreatedAt=> courseDto.CreatedAt;
        public DateTime? UpdateAt => courseDto.UpdateAt;
        public decimal? Rait => courseDto.Rait;
        public string Status 
        { 
            get
            {
                return courseDto.Status switch
                {
                    "Draft" => "В разработке",
                    "Published" => "Опубликован",
                    _ => "Ошибка",
                };
            }
        }
        public string ContentButton
        {
            get
            {
                return courseDto.Status switch
                {
                    "Draft" =>  "Продолжить создание",
                    "Published" => "Обновить",
                    _ => "Ошибка",
                };
                
            }
        }
    }
}
