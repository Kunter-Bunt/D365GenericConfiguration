using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace mwo.GenericConfiguration.DAL.Tests
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
            Cache = new CacheManager(nameof(CacheManagerTests), TimeSpan.FromMinutes(10.0));
        }

        [TestCleanup]
        public void Cleanup()
        {
            Cache.Clear();
        }

        [TestMethod]
        public void CacheManager_HasTest()
        {
            //Act
            var result = Cache.Has(Key);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CacheManager_SetHasTest()
        {
            //Arrange
            Cache.Set(Key, Value);

            //Act
            var result = Cache.Has(Key);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CacheManager_SetRemoveHasTest()
        {
            //Arrange
            Cache.Set(Key, Value);
            Cache.Remove(Key);

            //Act
            var result = Cache.Has(Key);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CacheManager_SetClearHasTest()
        {
            //Arrange
            Cache.Set(Key, Value);
            Cache.Clear();

            //Act
            var result = Cache.Has(Key);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CacheManager_SetExpireHasTest()
        {
            //Arrange
            Cache.Set(Key, Value, TimeSpan.FromMilliseconds(5));
            Thread.Sleep(10);

            //Act
            var result = Cache.Has(Key);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CacheManager_SetGetTest()
        {
            //Arrange
            Cache.Set(Key, Value);

            //Act
            var result = Cache.Get<string>(Key);

            //Assert
            Assert.AreEqual(Value, result);
        }

        [TestMethod]
        public void CacheManager_SetGetWrongTypeTest()
        {
            //Arrange
            Cache.Set(Key, Value);

            //Act
            var result = Cache.Get<int>(Key);

            //Assert
            Assert.AreEqual(default, result);
        }

        [TestMethod]
        public void CacheManager_GetTest()
        {
            //Act
            var result = Cache.Get<string>(Key);

            //Assert
            Assert.IsNull(result);
        }
    }
}