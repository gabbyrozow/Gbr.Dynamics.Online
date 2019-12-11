using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Gbr.Dynamics.Online.Utilities.Entities
{
    class EntityReferenceComparer : IEqualityComparer<EntityReference>
    {
        public bool Equals(EntityReference x, EntityReference y)
        {
            string identifierX = x.LogicalName + x.Id.ToString();
            string identifierY = y.LogicalName + y.Id.ToString();
            return identifierX.Equals(identifierY);
        }

        public int GetHashCode(EntityReference obj)
        {
            string identifier = obj.LogicalName + obj.Id.ToString();
            return identifier.GetHashCode();
        }
    }
}
