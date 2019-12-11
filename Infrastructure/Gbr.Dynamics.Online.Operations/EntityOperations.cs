using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Reflection;

namespace Gbr.Dynamics.Online.Operations
{
    public class EntityOperations : OperationsBase
    {
        #region Constructors

        public EntityOperations(IOrganizationService serviceProxy) : base(serviceProxy) { }

        #endregion

        #region Public Methods

        public Guid Create(Entity entity)
        {
            return ServiceProxy.Create(entity);
        }

        public void Update(Entity entity)
        {
            ServiceProxy.Update(entity);
        }

        public void Delete(string entityLogicalName, Guid id)
        {
            ServiceProxy.Delete(entityLogicalName, id);
        }

        public T GetRecord<T>(Guid id, string[] columnNames) where T : Entity
        {
            ColumnSet columns = new ColumnSet(columnNames);
            Entity result = ServiceProxy.Retrieve(GetEntityLogicalName<T>(), id, columns);
            return result.ToEntity<T>();
        }

        public T GetRecord<T>(string keyName, object keyValue, string[] columnNames) where T : Entity
        {
            RetrieveRequest request = new RetrieveRequest
            {
                ColumnSet = new ColumnSet(columnNames),
                Target = new EntityReference(GetEntityLogicalName<T>(), keyName, keyValue)
            };
            RetrieveResponse response = (RetrieveResponse)ServiceProxy.Execute(request);
            return response.Entity.ToEntity<T>();
        }

        public Entity GetRecord(String entityLogicalName, Guid id, string[] columnNames)
        {
            ColumnSet columns = new ColumnSet(columnNames);
            return ServiceProxy.Retrieve(entityLogicalName, id, columns);
        }

        public void Associate(EntityReference entityReference, EntityReferenceCollection associatedEntities, string relationshipName)
        {
            AssociateRequest request = new AssociateRequest
            {
                RelatedEntities = associatedEntities,
                Relationship = new Relationship(relationshipName),
                Target = entityReference
            };
            ServiceProxy.Execute(request);
        }

        public void Associate(EntityReference entityReference1, EntityReference entityReference2, string relationshipName)
        {
            AssociateRequest request = new AssociateRequest
            {
                RelatedEntities = new EntityReferenceCollection { entityReference2 },
                Relationship = new Relationship(relationshipName),
                Target = entityReference1
            };
            ServiceProxy.Execute(request);
        }

        public void Disassociate(EntityReference entityReference1, EntityReference entityReference2, string relationshipName)
        {
            DisassociateRequest request = new DisassociateRequest
            {
                RelatedEntities = new EntityReferenceCollection { entityReference2 },
                Relationship = new Relationship(relationshipName),
                Target = entityReference1
            };
            ServiceProxy.Execute(request);
        }

        public void Disassociate(EntityReference entityReference, EntityReferenceCollection associatedEntities, string relationshipName)
        {
            DisassociateRequest request = new DisassociateRequest
            {
                RelatedEntities = associatedEntities,
                Relationship = new Relationship(relationshipName),
                Target = entityReference
            };
            ServiceProxy.Execute(request);
        }

        public bool DoesUserHavePrivilage(EntityReference entity, EntityReference systemuser, AccessRights access)
        {
            RetrievePrincipalAccessRequest request = new RetrievePrincipalAccessRequest
            {
                Principal = systemuser,
                Target = entity
            };
            RetrievePrincipalAccessResponse principalAccessResponse = (RetrievePrincipalAccessResponse)ServiceProxy.Execute(request);
            if ((principalAccessResponse.AccessRights & access) != AccessRights.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GrantAccess(EntityReference principalEntityReference, EntityReference targetEntityReference, AccessRights accessMask)
        {
            GrantAccessRequest grantRequest = new GrantAccessRequest()
            {
                PrincipalAccess = new PrincipalAccess()
                {
                    Principal = principalEntityReference,
                    AccessMask = accessMask
                },
                Target = targetEntityReference
            };
            ServiceProxy.Execute(grantRequest);
        }

        public void AdduserToAccessTeam(Guid userId, EntityReference recordEntityReference, string teamTemplateName)
        {
            Guid templateTeamid = Guid.Empty;
            using (OrganizationServiceContext context = new OrganizationServiceContext(ServiceProxy))
            {
                templateTeamid = (Guid)(context.CreateQuery("teamtemplate").Single(tt => (string)tt["teamtemplatename"] == teamTemplateName)["teamtemplateid"]);
            }
            AddUserToRecordTeamRequest AddUserToRecordTeamRequest = new AddUserToRecordTeamRequest()
            {
                SystemUserId = userId,
                Record = recordEntityReference,
                TeamTemplateId = templateTeamid
            };
            ServiceProxy.Execute(AddUserToRecordTeamRequest);
        }

        public void RemoveUserFromAccessTeam(Guid userId, EntityReference recordEntityReference, string teamTemplateName)
        {
            Guid templateTeamid = Guid.Empty;
            using (OrganizationServiceContext context = new OrganizationServiceContext(ServiceProxy))
            {
                templateTeamid = (Guid)(context.CreateQuery("teamtemplate").Single(tt => (string)tt["teamtemplatename"] == teamTemplateName)["teamtemplateid"]);
            }
            RemoveUserFromRecordTeamRequest removeUserFromRecordTeamRequest = new RemoveUserFromRecordTeamRequest()
            {
                SystemUserId = userId,
                Record = recordEntityReference,
                TeamTemplateId = templateTeamid
            };
            ServiceProxy.Execute(removeUserFromRecordTeamRequest);
        }

        public void ModifyAccess(EntityReference principalEntityReference, EntityReference targetEntityReference, AccessRights accessMask)
        {
            ModifyAccessRequest modifyRequest = new ModifyAccessRequest()
            {
                PrincipalAccess = new PrincipalAccess()
                {
                    Principal = principalEntityReference,
                    AccessMask = accessMask
                },
                Target = targetEntityReference
            };
            ServiceProxy.Execute(modifyRequest);
        }

        public void RevokeAccess(EntityReference revokeeEntityReference, EntityReference targetEntityReference)
        {
            RevokeAccessRequest revokeRequest = new RevokeAccessRequest()
            {
                Revokee = revokeeEntityReference,
                Target = targetEntityReference
            };
            ServiceProxy.Execute(revokeRequest);
        }

        public void AddToQueue(Guid queueId, EntityReference target)
        {
            AddToQueueRequest request = new AddToQueueRequest
            {
                DestinationQueueId = queueId,
                Target = target
            };
            ServiceProxy.Execute(request);
        }

        #endregion

        #region Private Methods

        private string GetEntityLogicalName<T>() where T : Entity
        {
            Type entityType = typeof(T);
            FieldInfo nameField = entityType.GetField("EntityLogicalName");
            return (string)nameField.GetValue(null);
        }

        #endregion
    }
}
