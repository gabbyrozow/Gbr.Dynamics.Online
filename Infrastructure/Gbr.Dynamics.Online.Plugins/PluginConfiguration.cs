using Microsoft.Xrm.Sdk;

namespace Gbr.Dynamics.Online.Plugins
{
    public class PluginConfiguration
    {
        public IPluginExecutionContext Context { get; set; }
        public ITracingService TracingService { get; set; }
        public IOrganizationService ServiceProxy { get; set; }
    }
}
