using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials
{
    public class UpdateProgressMaterialCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }

        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
    }
}
