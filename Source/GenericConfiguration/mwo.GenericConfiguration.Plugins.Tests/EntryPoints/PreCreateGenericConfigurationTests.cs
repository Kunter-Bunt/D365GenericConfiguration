using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;

namespace mwo.GenericConfiguration.Plugins.EntryPoints.Tests
{
    [TestClass]
    public class PreValidationGenericConfigurationTests
    {
        XrmFakedContext ctx;
        readonly string key = nameof(key);

        [TestInitialize]
        public void Initialize()
        {
            ctx = new XrmFakedContext();
        }

        [DataRow("Value", mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow(null, mwo_GenericConfiguration_mwo_Type.Unspecified, true)]
        [DataRow(null, mwo_GenericConfiguration_mwo_Type.JSON, false)]
        [DataRow("Value", mwo_GenericConfiguration_mwo_Type.JSON, false)]
        [DataRow("{\"Key\": \"Value\"}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": 1}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": null}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": false}", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("{\"Key\": \"Value\",}", mwo_GenericConfiguration_mwo_Type.JSON, false)]
        [DataRow("[{\"Key\": \"Value\"}]", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("[1, 2]", mwo_GenericConfiguration_mwo_Type.JSON, true)]
        [DataRow("", mwo_GenericConfiguration_mwo_Type.CommaseparatedList, true)]
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
        [DataRow("<note><to>Tove</to><from>Jani</from><heading>Reminder</heading><body>Don't forget me this weekend!</body></note>", mwo_GenericConfiguration_mwo_Type.XML, true)]
        [DataRow("<note><to>Tove</to><from>Jani</from><heading>Reminder</heading><body>Don't forget me this weekend!</body>", mwo_GenericConfiguration_mwo_Type.XML, false)]
        [DataTestMethod]
        public void ExecuteCreateTest(string value, mwo_GenericConfiguration_mwo_Type type, bool shouldSuccceed)
        {
            var target = new mwo_GenericConfiguration { mwo_Key = key, mwo_Value = value, mwo_TypeEnum = type };

            try
            {
                ctx.ExecutePluginWithTarget<PreValidationGenericConfiguration>(target, MessageNameEnum.Create.ToString(), (int)StageEnum.PreValidation);
            }
            catch (InvalidPluginExecutionException)
            {
                if (shouldSuccceed) Assert.Fail("Plugin has thrown in Validation, while it should have succceeded.");
                return;
            }

            if (!shouldSuccceed) Assert.Fail("Plugin should have thrown in Validation, but didn't.");
        }
    }
}