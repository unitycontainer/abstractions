using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Lifetime;
using System;
using System.Threading;

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
        public virtual void GetSynchronizedValueTest()
        {
            var semaphor = new ManualResetEvent(false);

            new Thread(delegate ()
            {
                // Enter the lock
                _ = LifetimeManager.GetValue(LifetimeContainer);
                semaphor.Set();

                Thread.Sleep(100);

                // Act
                LifetimeManager.SetValue(TestObject, LifetimeContainer);
            }).Start();

            semaphor.WaitOne();
            SynchronizedLifetimeManager.ResolveTimeout = Timeout.Infinite;
            var value = LifetimeManager.GetValue(LifetimeContainer);

            Assert.AreSame(TestObject, value);
        }

        [TestMethod]
        public virtual void TryGetSynchronizedValueTest()
        {
            var semaphor = new ManualResetEvent(false);

            new Thread(delegate ()
            {
                // Enter the lock
                _ = LifetimeManager.GetValue(LifetimeContainer);

                semaphor.Set();
                Thread.Sleep(100);

                // Act
                LifetimeManager.SetValue(TestObject, LifetimeContainer);
            }).Start();

            semaphor.WaitOne();
            SynchronizedLifetimeManager.ResolveTimeout = Timeout.Infinite;
            var value = LifetimeManager.TryGetValue(LifetimeContainer);

            Assert.AreSame(LifetimeManager.NoValue, value);
        }

        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public virtual void GetSynchronizedValueTimeoutTest()
        {
            var semaphor = new ManualResetEvent(false);

            new Thread(delegate ()
            {
                // Enter the lock
                _ = LifetimeManager.GetValue(LifetimeContainer);
                semaphor.Set();

                Thread.Sleep(100);

                // Act
                LifetimeManager.SetValue(TestObject, LifetimeContainer);
            }).Start();

            semaphor.WaitOne();
            SynchronizedLifetimeManager.ResolveTimeout = 10;
            var value = LifetimeManager.GetValue(LifetimeContainer);
        }

        [TestMethod]
        public virtual void RecoverTest()
        {
            object value1 = null;
            object value2 = null;
            object value3 = null;

            Thread thread1 = new Thread(delegate ()
            {
                value1 = LifetimeManager.GetValue(LifetimeContainer);
                ((SynchronizedLifetimeManager)LifetimeManager).Recover();
            });

            Thread thread2 = new Thread(delegate ()
            {
                value2 = LifetimeManager.GetValue(LifetimeContainer);
                LifetimeManager.SetValue(TestObject, LifetimeContainer);
                value3 = LifetimeManager.GetValue(LifetimeContainer);
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Assert.AreSame(LifetimeManager.NoValue, value1);
            Assert.AreSame(LifetimeManager.NoValue, value2);
            Assert.AreSame(TestObject, value3);
        }

        [TestMethod]
        public virtual void RecoverWithNoLockTest()
        {
            object value1 = null;
            object value2 = null;

            Thread thread1 = new Thread(delegate ()
            {
                ((SynchronizedLifetimeManager)LifetimeManager).Recover();
            });

            Thread thread2 = new Thread(delegate ()
            {
                value1 = LifetimeManager.GetValue(LifetimeContainer);
                LifetimeManager.SetValue(TestObject, LifetimeContainer);
                value2 = LifetimeManager.GetValue(LifetimeContainer);
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Assert.AreSame(LifetimeManager.NoValue, value1);
            Assert.AreSame(TestObject, value2);
        }

        [Ignore]
        [TestMethod]
        public virtual void AddsDisposableToContainerTest()
        {
            // Arrange
            Assert.AreEqual(0, LifetimeContainer.Count);

            // Act
            LifetimeManager.SetValue(TestObject, LifetimeContainer);

            // Validate
            Assert.AreEqual(1, LifetimeContainer.Count);
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

        [TestMethod]
        public virtual void DisposedUnInitializedTest()
        {
            var manager = LifetimeManager as IDisposable;
            Assert.IsNotNull(manager);

            // Act
            manager.Dispose();
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
