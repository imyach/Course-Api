using Application.Common.Mappings;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Common.Commands.Materials.CreateMaterial
{
    public class CreateMaterialCommand : IRequest<Guid>
    {

        public Guid CurrentUserId{ get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
