using System.Activities;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using mwo.GenericConfiguration.Plugins.Models.CRM;

namespace mwo.GenericConfiguration.Plugins.EntryPoints
{
	[CrmPluginRegistration("mwo.GenericConfiguration.Plugins.EntryPoints.GenericConfiguationValueByKey", "1098ea41-b3a9-463a-8519-9f176dee6d39", 
		"Retrieve a Configuration by its Key.", "mwo.GenericConfiguration", IsolationModeEnum.Sandbox)]
	public class GenericConfiguationValueByKey : CodeActivity
	{
		[Input(nameof(Key))]
		public InArgument<string> Key { get; set; }

		[Output(nameof(Value))]
		public OutArgument<string> Value { get; set; }

		protected override void Execute(CodeActivityContext executionContext)
		{
			ITracingService trace = executionContext.GetExtension<ITracingService>();
			IWorkflowContext ctx = executionContext.GetExtension<IWorkflowContext>();
			IOrganizationServiceFactory factory = executionContext.GetExtension<IOrganizationServiceFactory>();

			string key = Key.Get<string>(executionContext);
			trace.Trace($"{mwo_GenericConfiguration.Fields.mwo_Key}: {key}");

			string value;
			using (CrmServiceContext crmUserContext = new CrmServiceContext(factory.CreateOrganizationService(ctx.UserId))) 
				value = crmUserContext.mwo_GenericConfigurationSet.Where(_ => _.mwo_Key == key).Select(_ => _.mwo_Value).FirstOrDefault();
			trace.Trace($"{mwo_GenericConfiguration.Fields.mwo_Value}: {value}");

			Value.Set(executionContext, value);
		}
	}
}