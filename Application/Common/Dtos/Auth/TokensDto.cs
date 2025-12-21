using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using Application.Common.Dtos.Users;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Auth
{
    public class TokensDto
    {
        public string AccessToken{ get; set; } = string.Empty;
        public Guid RefreshToken { get; set; }
    }
}
