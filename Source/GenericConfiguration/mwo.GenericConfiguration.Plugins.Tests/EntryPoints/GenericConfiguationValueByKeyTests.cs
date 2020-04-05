using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System.Activities;

namespace mwo.GenericConfiguration.Plugins.EntryPoints.Tests
{
    [TestClass]
    public class GenericConfiguationValueByKeyTests
    {
        private XrmFakedContext Context;
        private IOrganizationService Service;
        private const string Key = nameof(Key);
        private const string NotPersistedKey = nameof(NotPersistedKey);
        private const string Value = nameof(Value);

        [TestInitialize]
        public void Initialize()
        {
            Context = new XrmFakedContext();
            Service = Context.GetOrganizationService();
            Service.Create(new mwo_GenericConfiguration { mwo_Key = Key, mwo_Value = Value });
        }

        [DataRow(Key, Value)]
        [DataRow(NotPersistedKey, null)]
        [DataTestMethod]
        public void GCValueByKey_ExecuteTest(string keyToRetrieve, string expectedValue)
        {
            var inputs = new Dictionary<string, object> {
                { nameof(GenericConfiguationValueByKey.Key), keyToRetrieve }
            };

            var outputs = Context.ExecuteCodeActivity<GenericConfiguationValueByKey>(inputs);

            Assert.AreEqual(expectedValue, outputs[nameof(GenericConfiguationValueByKey.Value)]);
        }
    }
}