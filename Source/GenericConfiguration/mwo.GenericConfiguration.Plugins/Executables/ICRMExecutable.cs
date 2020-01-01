using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwo.GenericConfiguration.Plugins.Executables
{
    interface ICRMExecutable<T>
    {
        void Execute(CrmServiceContext ctx, ITracingService trace, T target, T preImage = default);
    }
}
