using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Executables;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwo.GenericConfiguration.Plugins.EntryPoints
{
    [CrmPluginRegistration(MessageNameEnum.Create, mwo_GenericConfiguration.EntityLogicalName, StageEnum.PreOperation, ExecutionModeEnum.Synchronous, "",
        "mwo.GenericConfiguration.Plugins.Entrypoints.PreCreateGenericConfiguration", 1, IsolationModeEnum.Sandbox)]
    class PreCreateGenericConfiguration : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            CrmServiceContext crmUserContext = new CrmServiceContext(factory.CreateOrganizationService(pluginExecutionContext.UserId));

            if (!pluginExecutionContext.InputParameters.ContainsKey("Target")
                || !(pluginExecutionContext.InputParameters["Target"] is mwo_GenericConfiguration))
            {
                tracingService.Trace("Context did not have a mwo_GenericConfiguration as Target, aborting.");
                return;
            }

            new GenericConfigurationValidator()
                .Execute(crmUserContext, tracingService, pluginExecutionContext.InputParameters["Target"] as mwo_GenericConfiguration);
        }
    }
}
