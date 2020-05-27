using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Unity.Lifetime;

namespace Lifetime.Managers
{
    [TestClass]
    public class HierarchicalManagerTests : SynchronizedManagerTests
    {
        protected override LifetimeManager GetManager() => new HierarchicalLifetimeManager();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public override void TryGetSetNoContainerTest()
        {
            base.TryGetSetNoContainerTest();
        }

        [TestMethod]
        public override void TryGetSetOtherContainerTest()
        {
            base.TryGetSetOtherContainerTest();

            // Validate
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(OtherContainer));
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(OtherContainer));

            // Act
            LifetimeManager.SetValue(TestObject, OtherContainer);

            // Validate
            Assert.AreSame(TestObject, LifetimeManager.TryGetValue(OtherContainer));
            Assert.AreSame(TestObject, LifetimeManager.GetValue(OtherContainer));
        }

        [TestMethod]
        public override void SetValueTwiceTest()
        {
            base.SetValueTwiceTest();
        }

        [TestMethod]
        public override void SetDifferentValuesTwiceTest()
        {
            base.SetDifferentValuesTwiceTest();
        }
    }
}
