using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;

namespace mwo.GenericConfiguration.Plugins.EntryPoints.Tests
{
    [TestClass()]
    public class GenericConfiguationValueByKeyTests
    {
        XrmFakedContext ctx;
        IOrganizationService svc;
        const string key = nameof(key);
        const string notpersistedKey = nameof(notpersistedKey);
        const string value = nameof(value);

        [TestInitialize]
        public void Initialize()
        {
            ctx = new XrmFakedContext();
            svc = ctx.GetOrganizationService();
            svc.Create(new mwo_GenericConfiguration { mwo_Key = key, mwo_Value = value });
        }

        [DataRow(key, value)]
        [DataRow(notpersistedKey, null)]
        [DataTestMethod]
        public void ExecuteTest(string keyToRetrieve, string expectedValue)
        {
            var inputs = new Dictionary<string, object> {
                { "Key", keyToRetrieve }
            };

            var outputs = ctx.ExecuteCodeActivity<GenericConfiguationValueByKey>(inputs);

            Assert.AreEqual(expectedValue, outputs["Value"]);
        }
    }
}