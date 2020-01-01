using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwo.GenericConfiguration.Plugins.Executables
{
    class GenericConfigurationValidator : ICRMExecutable<mwo_GenericConfiguration>
    {
        public void Execute(CrmServiceContext ctx, ITracingService trace, mwo_GenericConfiguration target, mwo_GenericConfiguration preImage = null)
        {
            
        }
    }
}
