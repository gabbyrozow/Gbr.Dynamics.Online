using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Gbr.Dynamics.Online.Utilities.Entities
{
    public static class EntityExtensions
    {
        public static object GetActualAttributeValue(this Entity target, Entity preImage, string attributeName)
        {
            object value = null;
            if (target != null && target.Contains(attributeName))
            {
                value = target[attributeName];
            }
            else if (preImage != null && preImage.Contains(attributeName))
            {
                value = preImage[attributeName];
            }
            return value;
        }

        public static string GetEntityReferenceName(this EntityReference target, string nameAttirbute, IOrganizationService serviceProxy)
        {
            string name;
            if (string.IsNullOrEmpty(target.Name) == true)
            {
                Entity referencedEntity = serviceProxy.Retrieve(target.LogicalName, target.Id, new ColumnSet(nameAttirbute));
                name = (string)referencedEntity[nameAttirbute];
            }
            else
            {
                name = target.Name;
            }
            return name;
        }
    }
}
