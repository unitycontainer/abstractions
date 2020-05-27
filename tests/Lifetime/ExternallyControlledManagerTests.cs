﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Unity.Lifetime;

namespace Lifetime.Managers
{
    [TestClass]
    public class ExternallyControlledManagerTests : SynchronizedManagerTests
    {
        protected override LifetimeManager GetManager() => new ExternallyControlledLifetimeManager();

        [TestMethod]
        public override void TryGetSetOtherContainerTest()
        {
            base.TryGetSetOtherContainerTest();

            // Validate
            Assert.AreSame(TestObject, TestManager.TryGetValue(OtherContainer));
            Assert.AreSame(TestObject, TestManager.GetValue(OtherContainer));
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

        [TestMethod]
        public override void IsDisposedTest()
        {
            // Arrange
            TestManager.SetValue(TestObject, LifetimeContainer);

            var manager = TestManager as IDisposable;
            var disposable = TestObject as FakeDisposable;

            Assert.IsNotNull(disposable);
            Assert.IsNotNull(manager);
            Assert.IsFalse(disposable.Disposed);

            // Act
            manager.Dispose();
            Assert.IsFalse(disposable.Disposed);
        }
    }
}
