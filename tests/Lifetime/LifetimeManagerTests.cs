using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity;
using Unity.Lifetime;

namespace Lifetime.Managers
{
    public abstract class LifetimeManagerTests
    {
        protected ILifetimeContainer LifetimeContainer;
        protected ILifetimeContainer OtherContainer;
        protected LifetimeManager LifetimeManager;

        protected object TestObject;

        [TestInitialize]
        public virtual void SetupTest()
        {
            LifetimeManager = GetManager();
            LifetimeContainer = new FakeLifetimeContainer();
            OtherContainer = new FakeLifetimeContainer();
            TestObject = new object();
        }

        #region   LifetimeManager Members

        [TestMethod]
        public virtual void CreateLifetimePolicyTest()
        {
            // Act
            var clone = LifetimeManager.CreateLifetimePolicy();

            // Validate
            Assert.IsInstanceOfType(clone, LifetimeManager.GetType());
        }

        [TestMethod]
        public virtual void InUseTest()
        {
            Assert.IsFalse(LifetimeManager.InUse);

            LifetimeManager.InUse = true;

            Assert.IsTrue(LifetimeManager.InUse);
        }

        [TestMethod]
        public virtual void TryGetValueTest()
        {
            // Validate
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));

            // Act
            LifetimeManager.SetValue(TestObject, LifetimeContainer);

            // Validate
            Assert.AreSame(TestObject, LifetimeManager.TryGetValue(LifetimeContainer));
            Assert.AreSame(TestObject, LifetimeManager.TryGetValue(LifetimeContainer));
        }

        [TestMethod]
        public virtual void GetValueTest()
        {
            // Validate
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));

            // Act
            LifetimeManager.SetValue(TestObject, LifetimeContainer);

            // Validate
            Assert.AreSame(TestObject, LifetimeManager.GetValue(LifetimeContainer));
            Assert.AreSame(TestObject, LifetimeManager.GetValue(LifetimeContainer));
        }

        [TestMethod]
        public virtual void TryGetSetOtherContainerTest()
        {
            // Validate
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));

            // Act
            LifetimeManager.SetValue(TestObject, LifetimeContainer);

            // Validate
            Assert.AreSame(TestObject, LifetimeManager.TryGetValue(LifetimeContainer));
            Assert.AreSame(TestObject, LifetimeManager.GetValue(LifetimeContainer));
        }

        [TestMethod]
        public virtual void TryGetSetNoContainerTest()
        {
            // Validate
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue());
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue());

            // Act
            LifetimeManager.SetValue(TestObject);

            // Validate
            Assert.AreSame(TestObject, LifetimeManager.TryGetValue());
            Assert.AreSame(TestObject, LifetimeManager.GetValue());
        }

        [TestMethod]
        public virtual void SetValueTwiceTest()
        {
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));

            // Act
            LifetimeManager.SetValue(TestObject, LifetimeContainer);
            LifetimeManager.SetValue(TestObject, LifetimeContainer);
        }

        [TestMethod]
        public virtual void SetDifferentValuesTwiceTest()
        {
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(LifetimeContainer));
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.TryGetValue(OtherContainer));
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(LifetimeContainer));
            Assert.AreSame(LifetimeManager.NoValue, LifetimeManager.GetValue(OtherContainer));

            // Act
            LifetimeManager.SetValue(TestObject, LifetimeContainer);
            LifetimeManager.SetValue(TestObject, OtherContainer);
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(LifetimeManager.ToString()));
        }

        #endregion


        #region Threading

        [TestMethod]
        public virtual void ValuesFromDifferentThreads()
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

            Assert.AreSame(TestObject, value1);
            Assert.AreSame(TestObject, value2);
            Assert.AreSame(TestObject, value3);
            Assert.AreSame(TestObject, value4);
        }

        #endregion


        #region Implementation

        protected abstract LifetimeManager GetManager();

        public class FakeLifetimeContainer : List<object>, ILifetimeContainer
        {
            public IUnityContainer Container => throw new System.NotImplementedException();

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            void ILifetimeContainer.Remove(object item) => Remove(item);
        }

        #endregion
    }
}
