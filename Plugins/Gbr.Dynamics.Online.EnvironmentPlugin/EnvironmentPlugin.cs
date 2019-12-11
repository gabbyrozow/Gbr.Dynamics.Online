using Gbr.Dynamics.Online.Plugins;
using Microsoft.Xrm.Sdk;
using System;

namespace Gbr.Dynamics.Online.EnvironmentPlugin
{
    public class EnvironmentPlugin : PluginBase, IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            RunPlugin(serviceProvider);
        }

        protected override PluginResult PreOperation(PluginConfiguration pluginConfiguration)
        {
            Actions actions = new Actions(pluginConfiguration.ServiceProxy);
            PluginResult result = new PluginResult { IsSuccess = true };
            switch (pluginConfiguration.Context.MessageName)
            {
                case "Create":
                    result = PreCreate(actions);
                    break;
            }
            return result;
        }

        private PluginResult PreCreate(Actions actions)
        {
            PluginResult result = new PluginResult { IsSuccess = true };
            bool IsEvironmentExists = actions.IsEvironmentExists();
            if(IsEvironmentExists)
            {
                result.IsSuccess = false;
                result.Message = "Cannot add more than one environment record";
            }
            return result;
        }
    }
}
