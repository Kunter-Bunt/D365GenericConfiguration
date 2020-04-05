using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;

namespace mwo.GenericConfiguration.Plugins.Executables
{
    interface ICRMExecutable<T>
    {
        void Execute(CrmServiceContext ctx, ITracingService trace, T target, T preImage = default);
    }
}
