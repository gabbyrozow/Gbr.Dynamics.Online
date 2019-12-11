using Microsoft.Xrm.Sdk;

namespace Gbr.Dynamics.Online.Operations
{
    public class OperationsBase
    {
        #region Properties

        public IOrganizationService ServiceProxy { get; set; }

        #endregion Properties

        #region Constructors

        public OperationsBase(IOrganizationService serviceProxy)
        {
            ServiceProxy = serviceProxy;
        }

        #endregion Constructors
    }
}
