using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Materials.DeleteMaterial
{
    public class DeleteMaterialCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
