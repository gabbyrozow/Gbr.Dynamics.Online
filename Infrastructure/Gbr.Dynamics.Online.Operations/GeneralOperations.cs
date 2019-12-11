using Gbr.Dynamics.Online.Utilities.Cryptography;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gbr.Dynamics.Online.Operations
{
    public class GeneralOperations : OperationsBase
    {
        #region Constructors

        public GeneralOperations(IOrganizationService serviceProxy) : base(serviceProxy) { }

        #endregion

        #region Public Methods

        internal Dictionary<string, string> GetConfigurationValues(string[] keys)
        {
            int environment = ((OptionSetValue)GetCurrentEnvironment()["gbr_currentenvironment"]).Value;
            Dictionary<string, string> configurationValues = new Dictionary<string, string>();
            foreach (string key in keys)
            {
                Entity configuration = GetConfigurationByKey(environment, key, new string[] { "gbr_configurationid", "gbr_name", "gbr_value", "gbr_isencrypted" });
                if ((bool)configuration["gbr_isencrypted"] == true)
                {
                    configurationValues.Add(key, ((string)configuration["gbr_value"]).Decrypt());
                }
                else
                {
                    configurationValues.Add(key, (string)configuration["gbr_value"]);
                }
            }
            return configurationValues;
        }

        public string GetConfigurationValue(string key)
        {
            return GetConfigurationValues(new string[] { key })[key];
        }

        public Dictionary<string, string> GetSystemMessages(string[] keys)
        {
            Dictionary<string, string> systemMessageValues = new Dictionary<string, string>();
            foreach (string key in keys)
            {
                Entity systemMessage = GetSystemMessageByKey(key, new string[] { "gbr_systemmessageid", "gbr_name", "gbr_value" });
                systemMessageValues.Add(key, (string)systemMessage["gbr_value"]);
            }
            return systemMessageValues;
        }

        public string GetSystemMessage(string key)
        {
            return GetSystemMessages(new string[] { key })[key];
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            return ServiceProxy.Execute(request);
        }

        public QueryExpression ConvertFetchToQuery(string fetchXml)
        {
            FetchXmlToQueryExpressionRequest conversionRequest = new FetchXmlToQueryExpressionRequest
            {
                FetchXml = fetchXml
            };
            FetchXmlToQueryExpressionResponse conversionResponse = (FetchXmlToQueryExpressionResponse)ServiceProxy.Execute(conversionRequest);
            return conversionResponse.Query;
        }

        public string ConvertQueryToFetch(QueryExpression queryExpression)
        {
            QueryExpressionToFetchXmlRequest req = new QueryExpressionToFetchXmlRequest
            {
                Query = queryExpression
            };
            QueryExpressionToFetchXmlResponse resp = (QueryExpressionToFetchXmlResponse)ServiceProxy.Execute(req);
            return resp.FetchXml;
        }

        public SendEmailResponse SendEmail(Guid emailId)
        {
            SendEmailRequest sendEmailreq = new SendEmailRequest
            {
                EmailId = emailId,
                TrackingToken = "",
                IssueSend = true
            };
            SendEmailResponse sendEmailresp = (SendEmailResponse)ServiceProxy.Execute(sendEmailreq);
            return sendEmailresp;
        }

        public Guid GetCurrentUserId()
        {
            WhoAmIResponse response = (WhoAmIResponse)ServiceProxy.Execute(new WhoAmIRequest());
            return response.UserId;
        }

        public string GetWebResourceContent(string webResourceName)
        {
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.Conditions.Add(new ConditionExpression("name", ConditionOperator.Equal, webResourceName));
            QueryExpression query = new QueryExpression
            {
                EntityName = "webresource",
                ColumnSet = new ColumnSet("content"),
                Criteria = filter,
                NoLock = true
            };
            EntityCollection entites = ServiceProxy.RetrieveMultiple(query);
            Entity webresource = (Entity)entites.Entities.SingleOrDefault();
            byte[] bytes = Convert.FromBase64String((string)webresource["content"]);
            string webResorceData = Encoding.UTF8.GetString(bytes);
            return webResorceData;
        }

        #endregion

        #region Private Methods

        private Entity GetCurrentEnvironment()
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = "gbr_environment",
                NoLock = true,
                ColumnSet = new ColumnSet(true)
            };
            EntityCollection environment = ServiceProxy.RetrieveMultiple(query);
            return environment.Entities.First();
        }

        private Entity GetConfigurationByKey(int environment, string key, string[] fields)
        {
            //Change to use alternate key (environment + key);
            QueryExpression query = new QueryExpression
            {
                EntityName = "gbr_configuration",
                ColumnSet = new ColumnSet(fields),
                Criteria = new FilterExpression(LogicalOperator.And),
                NoLock = true
            };
            query.Criteria.AddCondition(new ConditionExpression("gbr_name", ConditionOperator.Equal, key));
            query.Criteria.AddCondition(new ConditionExpression("gbr_environmentcode", ConditionOperator.Equal, environment));
            return ServiceProxy.RetrieveMultiple(query).Entities.First();
        }

        private Entity GetSystemMessageByKey(string key, string[] fields)
        {
            //Change to use alternate key (key)
            QueryExpression query = new QueryExpression
            {
                EntityName = "gbr_systemmessage",
                ColumnSet = new ColumnSet(fields),
                Criteria = new FilterExpression(LogicalOperator.And),
                NoLock = true
            };
            query.Criteria.AddCondition(new ConditionExpression("gbr_name", ConditionOperator.Equal, key));
            return ServiceProxy.RetrieveMultiple(query).Entities.First();
        }

        public ExecuteMultipleResponse ExecuteMultiple(OrganizationRequestCollection requests, bool ContinueOnError, bool ReturnResponses)
        {
            ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = ContinueOnError,
                    ReturnResponses = ReturnResponses
                },
                Requests = requests
            };
            return (ExecuteMultipleResponse)ServiceProxy.Execute(requestWithResults);
        }

        #endregion
    }
}
