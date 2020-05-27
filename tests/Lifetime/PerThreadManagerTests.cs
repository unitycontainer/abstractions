using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using Unity.Lifetime;

namespace Lifetime.Managers
{
    [TestClass]
    public class PerThreadManagerTests : LifetimeManagerTests
    {
        private object TestObject1;
        private object TestObject2;

        protected override LifetimeManager GetManager() => new PerThreadLifetimeManager();

        [TestInitialize]
        public override void SetupTest()
        {
            base.SetupTest();

            TestObject1 = new object();
            TestObject2 = new object();
        }

        [TestMethod]
        public override void TryGetValueTest()
        {
            Thread thread1 = new Thread(delegate ()
            {
                Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));

                // Act
                LifetimeManager.SetValue(TestObject1, LifetimeContainer);

                // Validate
                Assert.AreSame(TestObject1, LifetimeManager.TryGetValue(LifetimeContainer));
            })
            { Name = "1" };

            Thread thread2 = new Thread(delegate ()
            {
                Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));

                // Act
                LifetimeManager.SetValue(TestObject2, LifetimeContainer);

                // Validate
                Assert.AreSame(TestObject2, LifetimeManager.TryGetValue(LifetimeContainer));
            })
            { Name = "2" };

            thread1.Start();
            thread2.Start();

            thread2.Join();
            thread1.Join();

            base.TryGetValueTest();
        }

        [TestMethod]
        public override void GetValueTest()
        {
            Thread thread1 = new Thread(delegate ()
            {
                Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));

                // Act
                LifetimeManager.SetValue(TestObject1, LifetimeContainer);

                // Validate
                Assert.AreSame(TestObject1, LifetimeManager.GetValue(LifetimeContainer));
            })
            { Name = "1" };

            Thread thread2 = new Thread(delegate ()
            {
                Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));

                // Act
                LifetimeManager.SetValue(TestObject2, LifetimeContainer);

                // Validate
                Assert.AreSame(TestObject2, LifetimeManager.GetValue(LifetimeContainer));
            })
            { Name = "2" };

            thread1.Start();
            thread2.Start();

            thread2.Join();
            thread1.Join();

            base.GetValueTest();
        }

        [TestMethod]
        public override void TryGetSetOtherContainerTest()
        {
            Thread thread1 = new Thread(delegate ()
            {
                Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));
                Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));

                // Act
                LifetimeManager.SetValue(TestObject1, LifetimeContainer);

                // Validate
                Assert.AreSame(TestObject1, LifetimeManager.TryGetValue(LifetimeContainer));
                Assert.AreSame(TestObject1, LifetimeManager.GetValue(LifetimeContainer));

                Assert.AreSame(TestObject1, LifetimeManager.TryGetValue(OtherContainer));
                Assert.AreSame(TestObject1, LifetimeManager.GetValue(OtherContainer));
            })
            { Name = "1" };

            Thread thread2 = new Thread(delegate ()
            {
                Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));
                Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));

                // Act
                LifetimeManager.SetValue(TestObject2, LifetimeContainer);

                // Validate
                Assert.AreSame(TestObject2, LifetimeManager.TryGetValue(LifetimeContainer));
                Assert.AreSame(TestObject2, LifetimeManager.GetValue(LifetimeContainer));

                Assert.AreSame(TestObject1, LifetimeManager.TryGetValue(OtherContainer));
                Assert.AreSame(TestObject1, LifetimeManager.GetValue(OtherContainer));
            })
            { Name = "2" };

            thread1.Start();
            thread2.Start();

            thread2.Join();
            thread1.Join();

            base.TryGetSetOtherContainerTest();
        }

        [TestMethod]
        public override void ValuesFromDifferentThreads()
        {
            LifetimeManager.SetValue(TestObject, LifetimeContainer);

            object value1 = null;
            object value2 = null;
            object value3 = null;
            object value4 = null;

            Thread thread1 = new Thread(delegate ()
            {
                value1 = LifetimeManager.TryGetValue(LifetimeContainer);
                value2 = LifetimeManager.GetValue(LifetimeContainer);

            })
            { Name = "1" };

            Thread thread2 = new Thread(delegate ()
            {
                value3 = LifetimeManager.TryGetValue(LifetimeContainer);
                value4 = LifetimeManager.GetValue(LifetimeContainer);
            })
            { Name = "2" };

            thread1.Start();
            thread2.Start();

            thread2.Join();
            thread1.Join();

            Assert.AreSame(TestObject, LifetimeManager.TryGetValue(LifetimeContainer));
            Assert.AreSame(TestObject, LifetimeManager.GetValue(LifetimeContainer));

            Assert.AreSame(LifetimeManager.NoValue, value1);
            Assert.AreSame(LifetimeManager.NoValue, value2);
            Assert.AreSame(LifetimeManager.NoValue, value3);
            Assert.AreSame(LifetimeManager.NoValue, value4);
        }
    }
}
