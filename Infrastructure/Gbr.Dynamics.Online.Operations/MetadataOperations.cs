using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gbr.Dynamics.Online.Operations
{
    public class MetadataOperations : OperationsBase
    {
        #region Constructors

        public MetadataOperations(IOrganizationService serviceProxy) : base(serviceProxy) { }

        #endregion

        #region Public Methods

        public OptionMetadataCollection GetOptionsSetByAttribute(string entityName, string attributeName)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };
            var retrieveAttributeResponse = (RetrieveAttributeResponse)ServiceProxy.Execute(retrieveAttributeRequest);
            OptionMetadataCollection optionMetadataCollection = null;
            if (retrieveAttributeResponse.AttributeMetadata.GetType() == typeof(PicklistAttributeMetadata))
            {
                optionMetadataCollection = (((PicklistAttributeMetadata)retrieveAttributeResponse.AttributeMetadata).OptionSet).Options;
            }
            else if (retrieveAttributeResponse.AttributeMetadata.GetType() == typeof(StatusAttributeMetadata))
            {
                optionMetadataCollection = (((StatusAttributeMetadata)retrieveAttributeResponse.AttributeMetadata).OptionSet).Options;
            }
            return optionMetadataCollection;
        }

        public string GetOptionSetLabelByValue(string entityName, string attributeName, int value)
        {
            var optionMetadataCollection = GetOptionsSetByAttribute(entityName, attributeName);
            OptionMetadata option = optionMetadataCollection.SingleOrDefault(o => o.Value == value);
            string label = string.Empty;
            if (option != null)
            {
                label = option.Label.UserLocalizedLabel.Label;
            }
            return label;
        }
        public int GetEntityTypeCodeByEntityNames(string EntityName)
        {
            RetrieveEntityRequest request = new RetrieveEntityRequest
            {
                LogicalName = EntityName
            };
            RetrieveEntityResponse response = (RetrieveEntityResponse)ServiceProxy.Execute(request);
            return response.EntityMetadata.ObjectTypeCode.Value;
        }

        public Dictionary<int, string> GetEntityNamesByTypeCode()
        {
            RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest
            {
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            };
            RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)ServiceProxy.Execute(request);
            return response.EntityMetadata.ToDictionary(em => em.ObjectTypeCode.Value, em => em.LogicalName);
        }

        public AttributeMetadata GetAttributeDetails(string entityName, string attributeName)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)ServiceProxy.Execute(retrieveAttributeRequest);
            return retrieveAttributeResponse.AttributeMetadata;
        }

        public AttributeMetadata[] GetAttributeMetadataByEntityName(string entityName)
        {
            RetrieveEntityRequest request = new RetrieveEntityRequest()
            {
                LogicalName = entityName,
                EntityFilters = EntityFilters.Attributes
            };
            RetrieveEntityResponse response = (RetrieveEntityResponse)ServiceProxy.Execute(request);
            return response.EntityMetadata.Attributes;
        }

        #endregion
    }
}
