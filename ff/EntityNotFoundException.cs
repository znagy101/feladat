using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W31UL9_HSZF_2024252.Application
{
    public class EntityNotFoundException : Exception
    {
        public string EntityName { get; }
        public string EntityId { get; }

        public EntityNotFoundException(string entityName, int entitiyId) : base($"{entityName} nem találhato(ID: {entitiyId})")
        {
            EntityName = entityName;
            EntityId = EntityId;
        }
    }
}
