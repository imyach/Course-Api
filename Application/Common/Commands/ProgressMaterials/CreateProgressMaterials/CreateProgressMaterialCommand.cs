using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.CreateProgressMaterials
{
    public class CreateProgressMaterialCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }
        public Guid ProgressModuleId { get; set; }
        public Guid MaterialId { get; set; }
    }
}
