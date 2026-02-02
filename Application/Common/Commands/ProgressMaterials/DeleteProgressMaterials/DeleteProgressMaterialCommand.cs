using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.DeleteProgressMaterials
{
    public class DeleteProgressMaterialCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
