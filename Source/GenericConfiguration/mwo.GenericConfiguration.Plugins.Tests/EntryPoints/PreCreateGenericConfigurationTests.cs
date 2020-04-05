using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System.Collections.Generic;

namespace mwo.GenericConfiguration.Plugins.EntryPoints.Tests
{
    [TestClass]
    public class PreValidationGenericConfigurationTests
    {
        private XrmFakedContext Context;
        private IOrganizationService Service;
        private readonly string Key = nameof(Key);
        private const string OkXml = "<note><to>Tove</to><from>Jani</from><heading>Reminder</heading><body>Don't forget me this weekend!</body></note>";
        private const string BrokenXml = "<note><to>Tove</to><from>Jani</from><heading>Reminder</heading><body>Don't forget me this weekend!</body>";

        [TestInitialize]
        public void Initialize()
        {
            Context = new XrmFakedContext();
            Service = Context.GetOrganizationService();
        }

        [DataRow("Value", mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow(null, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow(null, mwo_GenericConfiguration_mwo_Type.JSON, false)]
        [DataRow("Value", mwo_GenericConfiguration_mwo_Type.JSON, false)]
        [DataRow("{\"Key\": \"Value\"}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": 1}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": null}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": false}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": [1,2]}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": \"Value\",}", mwo_GenericConfiguration_mwo_Type.JSON, false)]
        [DataRow("[{\"Key\": \"Value\"}]", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("[1, 2]", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow(null, mwo_GenericConfiguration_mwo_Type.CommaseparatedList, true)]
        [DataRow("1", mwo_GenericConfiguration_mwo_Type.CommaseparatedList, true)]
        [DataRow("1,", mwo_GenericConfiguration_mwo_Type.CommaseparatedList, false)]
        [DataRow("1,2", mwo_GenericConfiguration_mwo_Type.CommaseparatedList, true)]
        [DataRow("1;2", mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList, true)]
        [DataRow("1;", mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList, false)]
        [DataRow("1", mwo_GenericConfiguration_mwo_Type.Number, true)]
        [DataRow("1.2", mwo_GenericConfiguration_mwo_Type.Number, true)]
        [DataRow(null, mwo_GenericConfiguration_mwo_Type.Number, false)]
        [DataRow("Value", mwo_GenericConfiguration_mwo_Type.Number, false)]
        [DataRow("Value", mwo_GenericConfiguration_mwo_Type.Boolean, false)]
        [DataRow(null, mwo_GenericConfiguration_mwo_Type.Boolean, false)]
        [DataRow("True", mwo_GenericConfiguration_mwo_Type.Boolean, true)]
        [DataRow("False", mwo_GenericConfiguration_mwo_Type.Boolean, true)]
        [DataRow("Value", mwo_GenericConfiguration_mwo_Type.XML, false)]
        [DataRow(null, mwo_GenericConfiguration_mwo_Type.XML, false)]
        [DataRow(OkXml, mwo_GenericConfiguration_mwo_Type.XML, true)]
        [DataRow(BrokenXml, mwo_GenericConfiguration_mwo_Type.XML, false)]
        [DataTestMethod]
        public void ExecuteCreateTest(string value, mwo_GenericConfiguration_mwo_Type type, bool shouldSuccceed)
        {
            var target = new mwo_GenericConfiguration { mwo_Key = Key, mwo_Value = value, mwo_TypeEnum = type };

            try
            {
                Context.ExecutePluginWithTarget<PreValidationGenericConfiguration>(target, MessageNameEnum.Create.ToString(), (int)StageEnum.PreValidation);
            }
            catch (InvalidPluginExecutionException)
            {
                if (shouldSuccceed) Assert.Fail("Plugin has thrown in Validation, while it should have succceeded.");
                return;
            }

            if (!shouldSuccceed) Assert.Fail("Plugin should have thrown in Validation, but didn't.");
        }

        //Type changes
        [DataRow(null, null, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.JSON, false)]
        [DataRow(null, null, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.XML, false)]
        [DataRow(null, null, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.Number, false)]
        [DataRow(null, null, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.CommaseparatedList, true)]
        [DataRow(null, null, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList, true)]
        [DataRow(null, null, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.Boolean, false)]
        [DataRow("[1, 2]", "[1, 2]", mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("[1, 2]", "[1, 2]", mwo_GenericConfiguration_mwo_Type.CommaseparatedList, mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("1", "1", mwo_GenericConfiguration_mwo_Type.Number, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow("1,2", "1,2", mwo_GenericConfiguration_mwo_Type.CommaseparatedList, mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList, true)]
        [DataRow("1;2", "1;2", mwo_GenericConfiguration_mwo_Type.CommaseparatedList, mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList, true)]
        [DataRow("True", "True", mwo_GenericConfiguration_mwo_Type.Boolean, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow("False", "False", mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.Boolean   , true)]
        [DataRow(OkXml, OkXml, mwo_GenericConfiguration_mwo_Type.XML, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow(BrokenXml, BrokenXml, mwo_GenericConfiguration_mwo_Type.XML, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow(BrokenXml, BrokenXml, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.XML, false)]
        //Value changes
        [DataRow(BrokenXml, OkXml, mwo_GenericConfiguration_mwo_Type.XML, mwo_GenericConfiguration_mwo_Type.XML, true)]
        [DataRow(OkXml, BrokenXml, mwo_GenericConfiguration_mwo_Type.XML, mwo_GenericConfiguration_mwo_Type.XML, false)]
        [DataRow("[1, 2]", "[{\"Key\": \"Value\"}]", mwo_GenericConfiguration_mwo_Type.JSON, mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("[1, 2]", OkXml, mwo_GenericConfiguration_mwo_Type.JSON, mwo_GenericConfiguration_mwo_Type.JSON, false)]
        [DataRow("1,2", "1;2", mwo_GenericConfiguration_mwo_Type.CommaseparatedList, mwo_GenericConfiguration_mwo_Type.CommaseparatedList, true)]
        [DataRow("1,2", "1;2", mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList, mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList, true)]
        [DataRow("True", "False", mwo_GenericConfiguration_mwo_Type.Boolean, mwo_GenericConfiguration_mwo_Type.Boolean, true)]
        [DataRow("True", "false", mwo_GenericConfiguration_mwo_Type.Boolean, mwo_GenericConfiguration_mwo_Type.Boolean, true)]
        [DataRow("1", "1.2", mwo_GenericConfiguration_mwo_Type.Number, mwo_GenericConfiguration_mwo_Type.Number, true)]
        [DataRow("1", "1-2", mwo_GenericConfiguration_mwo_Type.Number, mwo_GenericConfiguration_mwo_Type.Number, false)]
        [DataRow("1", "Value", mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        //Both change
        [DataRow("1", "Value", mwo_GenericConfiguration_mwo_Type.Number, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow("[1, 2]", OkXml, mwo_GenericConfiguration_mwo_Type.JSON, mwo_GenericConfiguration_mwo_Type.XML, true)]
        [DataRow("[1, 2]", OkXml, mwo_GenericConfiguration_mwo_Type.JSON, mwo_GenericConfiguration_mwo_Type.Number, false)]
        [DataRow(OkXml, BrokenXml, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.XML, false)]
        [DataRow(OkXml, OkXml, mwo_GenericConfiguration_mwo_Type.Unspecified, mwo_GenericConfiguration_mwo_Type.XML, true)]
        [DataRow(OkXml, BrokenXml, mwo_GenericConfiguration_mwo_Type.XML, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataTestMethod]
        public void ExecuteUpdateTest(string oldValue, string newValue, mwo_GenericConfiguration_mwo_Type oldType, mwo_GenericConfiguration_mwo_Type newType, bool shouldSuccceed)
        {
            var preImage = new mwo_GenericConfiguration { mwo_Key = Key, mwo_Value = oldValue, mwo_TypeEnum = oldType };
            preImage.Id = Service.Create(preImage);

            var target = new mwo_GenericConfiguration { Id = preImage.Id };
            if (!string.Equals(newValue, oldValue)) target.mwo_Value = newValue;
            if (oldType != newType) target.mwo_TypeEnum = newType;
            XrmFakedPluginExecutionContext pluginContext = SetupPluginExecutioncontext(target, preImage);

            try
            {
                Context.ExecutePluginWith<PreValidationGenericConfiguration>(pluginContext);
            }
            catch (InvalidPluginExecutionException)
            {
                if (shouldSuccceed) Assert.Fail("Plugin has thrown in Validation, while it should have succceeded.");
                return;
            }

            if (!shouldSuccceed) Assert.Fail("Plugin should have thrown in Validation, but didn't.");
        }

        private XrmFakedPluginExecutionContext SetupPluginExecutioncontext(mwo_GenericConfiguration target, mwo_GenericConfiguration preImage)
        {
            var pluginExecutionContext = Context.GetDefaultPluginContext();
            pluginExecutionContext.MessageName = MessageNameEnum.Update.ToString();
            pluginExecutionContext.Stage = (int)StageEnum.PreValidation;
            pluginExecutionContext.InputParameters = new ParameterCollection {
                new KeyValuePair<string, object>(PreValidationGenericConfiguration.TargetName, target)
            };

            if (preImage != null)
            {
                pluginExecutionContext.PreEntityImages = new EntityImageCollection() {
                    new KeyValuePair<string, Entity> (PreValidationGenericConfiguration.PreImageName, preImage)
                };
            }

            return pluginExecutionContext;
        }
    }
}