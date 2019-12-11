using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.WebServiceClient;
using System;
using System.Threading.Tasks;

namespace Gbr.Dynamics.Online.Client
{
    public static class ServiceFactory
    {
        #region Public Methods

        public static IOrganizationService GetServiceFactory(ServiceConfiguration serviceConfiguration)
        {
            OrganizationWebProxyClient serviceProxy = new OrganizationWebProxyClient(new Uri(serviceConfiguration.ServiceUrl), true)
            {
                HeaderToken = GetAccessToken(serviceConfiguration.ClientId, serviceConfiguration.ClientSecret, serviceConfiguration.AadInstance, serviceConfiguration.TenantId, serviceConfiguration.OrganizationUrl).Result
            };
            return serviceProxy;
        }

        public static IOrganizationService GetServiceFactory(ServiceConfiguration serviceConfiguration, Guid userId)
        {
            OrganizationWebProxyClient serviceProxy = new OrganizationWebProxyClient(new Uri(serviceConfiguration.ServiceUrl), true)
            {
                HeaderToken = GetAccessToken(serviceConfiguration.ClientId, serviceConfiguration.ClientSecret, serviceConfiguration.AadInstance, serviceConfiguration.TenantId, serviceConfiguration.OrganizationUrl).Result,
                CallerId = userId
            };
            return serviceProxy;
        }

        #endregion Public Methods

        #region Private Methods

        private static async Task<string> GetAccessToken(string clientId, string clientSecret, string aadInstance, string tenantId, string organizationUrl)
        {
            ClientCredential credential = new ClientCredential(clientId, clientSecret);
            AuthenticationContext authenticationContext = new AuthenticationContext(aadInstance + tenantId);
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(organizationUrl, credential);
            return authenticationResult.AccessToken;
        }

        #endregion Private Methods
    }
}
