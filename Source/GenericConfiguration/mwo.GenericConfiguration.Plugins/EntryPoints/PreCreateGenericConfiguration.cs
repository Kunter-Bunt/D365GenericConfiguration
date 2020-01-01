using Microsoft.Xrm.Sdk;
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
            throw new NotImplementedException();
        }
    }
}
