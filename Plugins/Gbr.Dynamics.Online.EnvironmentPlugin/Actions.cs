using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Gbr.Dynamics.Online.EnvironmentPlugin
{
    class Actions
    {
        #region Variables

        private readonly IOrganizationService serviceProxy;

        #endregion

        #region Constructor

        public Actions(IOrganizationService serviceProxy)
        {
            this.serviceProxy = serviceProxy;
        }

        #endregion

        #region Internal Methods

        internal bool IsEvironmentExists()
        {
            QueryExpression query = new QueryExpression
            {
                ColumnSet = new ColumnSet("gbr_environmentid"),
                EntityName = "gbr_environment"
            };
            EntityCollection results = serviceProxy.RetrieveMultiple(query);
            bool isEvironmentExists = results.Entities.Count > 0;
            return isEvironmentExists;
        }

        #endregion
    }
}
