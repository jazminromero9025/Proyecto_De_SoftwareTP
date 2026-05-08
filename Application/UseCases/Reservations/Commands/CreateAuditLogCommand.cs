using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Reservations.Commands
{
    public class CreateAuditLogCommand
    {
        public int? UserId { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Details { get; set; }

        public CreateAuditLogCommand(int? userId, string action, string entityType, string entityId, string details)
        {
            UserId = userId;
            Action = action;
            EntityType = entityType;
            EntityId = entityId;
            Details = details;
        }




    }
}
