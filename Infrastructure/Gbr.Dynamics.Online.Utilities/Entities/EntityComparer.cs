using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Gbr.Dynamics.Online.Utilities.Entities
{
    public class EntityComparer : IEqualityComparer<Entity>
    {
        public bool Equals(Entity x, Entity y)
        {
            string identifierX = x.LogicalName + x.Id.ToString();
            string identifierY = y.LogicalName + y.Id.ToString();
            return identifierX.Equals(identifierY);
        }

        public int GetHashCode(Entity obj)
        {
            string identifier = obj.LogicalName + obj.Id.ToString();
            return identifier.GetHashCode();
        }
    }
}
