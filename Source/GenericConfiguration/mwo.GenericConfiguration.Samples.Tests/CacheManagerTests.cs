using Microsoft.VisualStudio.TestTools.UnitTesting;
using mwo.GenericConfiguration.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwo.GenericConfiguration.Samples.Tests
{
    [TestClass]
    public class CacheManagerTests
    {
        private ICache Cache;
        private const string Key = nameof(Key);
        private const string Value = nameof(Value);

        [TestInitialize]
        public void Initialize()
        {
            Cache = new CacheManager(nameof(CacheManagerTests));
        }

        [TestCleanup]
        public void Cleanup()
        {
            Cache.Clear();
        }

        [TestMethod]
        public void CacheManager_HasTest()
        {
            //Arrange
            Cache.Set(Key, Value);

            //Act
            var result = Cache.Has(Key);

            //Assert
            Assert.IsTrue(result);
        }
    }
}