using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Lifetime;
using System;

namespace Lifetime.Managers
{
    [TestClass]
    public abstract class SynchronizedManagerTests : LifetimeManagerTests
    {
        [TestInitialize]
        public override void SetupTest()
        {
            base.SetupTest();
            TestObject = new FakeDisposable();
        }

        [TestMethod]
        public virtual void IsDisposedTest()
        {
            // Arrange
            LifetimeManager.SetValue(TestObject, LifetimeContainer);

            var manager    = LifetimeManager as IDisposable;
            var disposable = TestObject as FakeDisposable;

            Assert.IsNotNull(disposable);
            Assert.IsNotNull(manager);
            Assert.IsFalse(disposable.Disposed);

            // Act
            manager.Dispose();
            Assert.IsTrue(disposable.Disposed);
        }

        public class FakeDisposable : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                Disposed = true;
            }
        }
    }
}
