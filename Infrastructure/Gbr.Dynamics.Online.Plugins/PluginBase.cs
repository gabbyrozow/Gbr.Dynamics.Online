using Gbr.Dynamics.Online.Operations;
using Microsoft.Xrm.Sdk;
using System;

namespace Gbr.Dynamics.Online.Plugins
{
    public class PluginBase
    {
        #region Methods

        protected PluginConfiguration GetPluginConfiguration(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            PluginConfiguration pluginConfiguration = new PluginConfiguration
            {
                Context = context,
                TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService)),
                ServiceProxy = GetOrganizationServiceProxy(serviceProvider, context.UserId)
            };
            return pluginConfiguration;
        }

        protected IOrganizationService GetAdministratorServiceProxy(IOrganizationService userServiceProxy, IServiceProvider serviceProvider)
        {
            GeneralOperations generalOperations = new GeneralOperations(userServiceProxy);
            Guid adminUserId = new Guid(generalOperations.GetConfigurationValue("AdminUserId"));
            return GetOrganizationServiceProxy(serviceProvider, adminUserId);
        }

        protected void ThrowWarningMessage(string message, int contextMode)
        {
            if (contextMode == 0)
            {
                throw new InvalidPluginExecutionException(message);
            }
        }

        protected IOrganizationService GetOrganizationServiceProxy(IServiceProvider serviceProvider, Guid userId)
        {
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService proxy = factory.CreateOrganizationService(userId);
            return proxy;
        }

        protected void RunPlugin(IServiceProvider serviceProvider)
        {
            PluginConfiguration pluginConfiguration = GetPluginConfiguration(serviceProvider);
            PluginResult result = new PluginResult { IsSuccess = true };
            try
            {
                switch (pluginConfiguration.Context.Mode)
                {
                    case (int)MessageProcessingMode.Asynchronous:
                        AsyncOperation(pluginConfiguration);
                        break;
                    case (int)MessageProcessingMode.Synchronous:
                        switch (pluginConfiguration.Context.Stage)
                        {
                            case (int)MessageProcessingStage.PreValidation:
                                result = PreValidation(pluginConfiguration);
                                break;
                            case (int)MessageProcessingStage.PreOperation:
                                result = PreOperation(pluginConfiguration);
                                break;
                            case (int)MessageProcessingStage.PostOperation:
                                PostOperation(pluginConfiguration);
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                pluginConfiguration.TracingService.Trace(ex.ToString());
                throw;
            }
            finally
            {
                if (result.IsSuccess == false)
                {
                    ThrowWarningMessage(result.Message, pluginConfiguration.Context.Mode);
                }
            }
        }

        #endregion

        #region Virtual Methods

        protected virtual void AsyncOperation(PluginConfiguration pluginConfiguration) { }

        protected virtual PluginResult PreValidation(PluginConfiguration pluginConfiguration)
        {
            return new PluginResult { IsSuccess = true };
        }

        protected virtual PluginResult PreOperation(PluginConfiguration pluginConfiguration)
        {
            return new PluginResult { IsSuccess = true };
        }

        protected virtual void PostOperation(PluginConfiguration pluginConfiguration) { }

        #endregion
    }
}
