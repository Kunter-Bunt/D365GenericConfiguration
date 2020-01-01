using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Executables;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System;

namespace mwo.GenericConfiguration.Plugins.EntryPoints
{
    /// <summary>
    /// Plugin Type Class to be registered in D365.
    /// </summary>
    [CrmPluginRegistration(MessageNameEnum.Create, mwo_GenericConfiguration.EntityLogicalName, 
        StageEnum.PreValidation, ExecutionModeEnum.Synchronous, "",
        "mwo.GenericConfiguration.Plugins.Entrypoints.PreValidationCreateGenericConfiguration", 1, IsolationModeEnum.Sandbox)]    
    [CrmPluginRegistration(MessageNameEnum.Update, mwo_GenericConfiguration.EntityLogicalName, 
        StageEnum.PreValidation, ExecutionModeEnum.Synchronous, "mwo_type,mwo_value",
        "mwo.GenericConfiguration.Plugins.Entrypoints.PreValidationUpdateGenericConfiguration", 1, IsolationModeEnum.Sandbox, 
        Image1Type = ImageTypeEnum.PreImage, Image1Name = "Default", Image1Attributes = "mwo_type,mwo_value")]
    public class PreValidationGenericConfiguration : IPlugin
    {
        /// <summary>
        /// Entrypoint for D365, will take the MS service provider and hand of to an executable.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new InvalidPluginExecutionException(nameof(serviceProvider) + Errors.NullError);
            IPluginExecutionContext pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            mwo_GenericConfiguration target;
            if (pluginExecutionContext.InputParameters.ContainsKey("Target")
                && (pluginExecutionContext.InputParameters["Target"] is Entity))
                target = ((Entity)pluginExecutionContext.InputParameters["Target"]).ToEntity<mwo_GenericConfiguration>();
            else
            {
                tracingService.Trace("Context did not have a mwo_GenericConfiguration as Target, aborting.");
                return;
            }

            mwo_GenericConfiguration preImage = null;
            if (pluginExecutionContext.PreEntityImages.ContainsKey("Default")
                && (pluginExecutionContext.PreEntityImages["Default"] is Entity))
                preImage = pluginExecutionContext.PreEntityImages["Default"].ToEntity<mwo_GenericConfiguration>();

            using (CrmServiceContext crmUserContext = new CrmServiceContext(factory.CreateOrganizationService(pluginExecutionContext.UserId)))
                new GenericConfigurationValidator().Execute(crmUserContext, tracingService, target, preImage);
        }
    }
}