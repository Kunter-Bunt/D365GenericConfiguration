using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mwo.GenericConfiguration.Plugins.Models.CRM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace mwo.GenericConfiguration.Samples.Tests
{
    public class XmlSerializable
    {
        public string Name;
    }

    [TestClass]
    public class GenericConfigurationReaderTests
    {
        private readonly string NotPersistedKey = nameof(NotPersistedKey);
        private readonly string DefaultKey = nameof(DefaultKey);
        private readonly string DefaultValue = nameof(DefaultValue);
        private readonly string NumberKey = nameof(NumberKey);
        private readonly double NumberValue = 10.0;
        private readonly string BooleanKey = nameof(BooleanKey);
        private readonly bool BooleanValue = true;
        private readonly string JSONKey = nameof(JSONKey);
        private readonly Dictionary<string, string> JSONValue = new Dictionary<string, string> { { nameof(JSONKey), nameof(JSONValue) } };
        private readonly string BrokenJSONKey = nameof(BrokenJSONKey);
        private readonly string BrokenJSONValue = "{\"Key\": \"Value\", asdf}";
        private readonly string XMLKey = nameof(XMLKey);
        private readonly XmlSerializable XMLValue = new XmlSerializable { Name = nameof(XMLValue) };
        private readonly string SemicolonListKey = nameof(SemicolonListKey);
        private readonly List<int> SemicolonListValue = new List<int> { 1, 2, 3 };
        private readonly string CommaListKey = nameof(CommaListKey);
        private readonly List<string> CommaListValue = new List<string> { "a", "b", "c" };

        private CrmServiceContext Context;
        private GenericConfigurationReader Reader;

        [TestInitialize]
        public void Initialize()
        {
            var xmlSerializer = new XmlSerializer(XMLValue.GetType());
            var writer = new StringWriter();
            xmlSerializer.Serialize(writer, XMLValue);

            var fakectx = new XrmFakedContext();
            Context = new CrmServiceContext(fakectx.GetOrganizationService());

            Context.AddObject(new mwo_GenericConfiguration { mwo_Key = DefaultKey, mwo_Value = DefaultValue, mwo_TypeEnum = mwo_GenericConfiguration_mwo_Type.Unspecified });
            Context.AddObject(new mwo_GenericConfiguration { mwo_Key = NumberKey, mwo_Value = NumberValue.ToString(), mwo_TypeEnum = mwo_GenericConfiguration_mwo_Type.Number });
            Context.AddObject(new mwo_GenericConfiguration { mwo_Key = BooleanKey, mwo_Value = BooleanValue.ToString(), mwo_TypeEnum = mwo_GenericConfiguration_mwo_Type.Boolean });
            Context.AddObject(new mwo_GenericConfiguration { mwo_Key = BrokenJSONKey, mwo_Value = BrokenJSONValue, mwo_TypeEnum = mwo_GenericConfiguration_mwo_Type.Unspecified });
            Context.AddObject(new mwo_GenericConfiguration { mwo_Key = JSONKey, mwo_Value = new JavaScriptSerializer().Serialize(JSONValue), mwo_TypeEnum = mwo_GenericConfiguration_mwo_Type.JSON });
            Context.AddObject(new mwo_GenericConfiguration { mwo_Key = XMLKey, mwo_Value = writer.ToString(), mwo_TypeEnum = mwo_GenericConfiguration_mwo_Type.XML });
            Context.AddObject(new mwo_GenericConfiguration { mwo_Key = SemicolonListKey, mwo_Value = string.Join(";", SemicolonListValue), mwo_TypeEnum = mwo_GenericConfiguration_mwo_Type.SemicolonseparatedList });
            Context.AddObject(new mwo_GenericConfiguration { mwo_Key = CommaListKey, mwo_Value = string.Join(",", CommaListValue), mwo_TypeEnum = mwo_GenericConfiguration_mwo_Type.CommaseparatedList });
            Context.SaveChanges();

            Reader = new GenericConfigurationReader(Context, fakectx.GetFakeTracingService());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            new CacheManager(nameof(GenericConfigurationReader), TimeSpan.MinValue).Clear();
        }

        [TestMethod]
        public void GCReader_GetStringTest()
        {
            //Act
            var result = Reader.GetString(DefaultKey, string.Empty);

            //Assert
            Assert.AreEqual(DefaultValue, result);
        }

        [TestMethod]
        public void GCReader_GetStringTwiceTest()
        {
            //Arrange
            Reader.GetString(DefaultKey, string.Empty);

            //Act
            var result = Reader.GetString(DefaultKey, string.Empty);

            //Assert
            Assert.AreEqual(DefaultValue, result);
        }

        [TestMethod]
        public void GCReader_GetStringDefaultTest()
        {
            //Act
            var result = Reader.GetString(NotPersistedKey, DefaultValue);

            //Assert
            Assert.AreEqual(DefaultValue, result);
        }

        [TestMethod]
        public void GCReader_GetBoolTest()
        {
            //Act
            var result = Reader.GetBool(BooleanKey, null);

            //Assert
            Assert.AreEqual(BooleanValue, result);
        }

        [TestMethod]
        public void GCReader_GetBoolgDefaultTest()
        {
            //Act
            var result = Reader.GetBool(NotPersistedKey, BooleanValue);

            //Assert
            Assert.AreEqual(BooleanValue, result);
        }

        [TestMethod]
        public void GCReader_GetNumberTest()
        {
            //Act
            var result = Reader.GetNumber(NumberKey, double.NaN);

            //Assert
            Assert.AreEqual(NumberValue, result);
        }

        [TestMethod]
        public void GCReader_GetNumberDefaultTest()
        {
            //Act
            var result = Reader.GetNumber(NotPersistedKey, NumberValue);

            //Assert
            Assert.AreEqual(NumberValue, result);
        }

        [TestMethod]
        public void GCReader_GetListCommaTest()
        {
            //Act
            var result = Reader.GetList(CommaListKey, null);

            //Assert
            Assert.AreEqual(CommaListValue.First(), result.First());
        }

        [TestMethod]
        public void GCReader_GetListSemicolonTest()
        {
            //Act
            var result = Reader.GetList(SemicolonListKey, null);

            //Assert
            Assert.AreEqual(SemicolonListValue.First().ToString(), result.First());
        }

        [TestMethod]
        public void GCReader_GetListDefaultValueTest()
        {
            //Act
            var result = Reader.GetList(NotPersistedKey, CommaListValue);

            //Assert
            Assert.AreEqual(CommaListValue.First(), result.First());
        }

        [TestMethod]
        public void GCReader_GetJSONTest()
        {
            //Act
            var result = Reader.GetObject<Dictionary<string,string>>(JSONKey, null);

            //Assert
            Assert.AreEqual(JSONValue.Keys.First(), result.Keys.First());
        }


        [TestMethod]
        public void GCReader_GetXmlTest()
        {
            //Act
            var result = Reader.GetObject<XmlSerializable>(XMLKey, null);

            //Assert
            Assert.AreEqual(XMLValue.Name, result.Name);
        }

        [TestMethod]
        public void GCReader_GetObjectNotExistingTest()
        {
            //Act
            var result = Reader.GetObject(NotPersistedKey, JSONValue);

            //Assert
            Assert.AreEqual(JSONValue.Keys.First(), result.Keys.First());
        }

        [TestMethod]
        public void GCReader_GetObjectNotValidTest()
        {
            //Act
            var result = Reader.GetObject(DefaultKey, JSONValue);

            //Assert
            Assert.AreEqual(JSONValue.Keys.First(), result.Keys.First());
        }

        [TestMethod]
        public void GCReader_GetJSONBrokenTest()
        {
            //Act
            var result = Reader.GetObject(BrokenJSONKey, JSONValue);

            //Assert
            Assert.AreEqual(JSONValue.Keys.First(), result.Keys.First());
        }
    }
}