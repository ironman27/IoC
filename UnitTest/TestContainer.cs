using System;
using System.Reflection;
using IoC.Container;
using IoC.Container.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Examples;

namespace UnitTest
{
    [TestClass]
    public class TestContainer
    {
        private Container _container;

        [TestInitialize]
        public void Init()
        {
            _container = new Container(new Creator());
        }

        [TestMethod]
        public void GenericConstructorInjectionTest()
        {
            _container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBll = _container.CreateInstance<CustomerBLL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
        }

        [TestMethod]
        public void ConstructorInjectionTest()
        {
            _container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBll = _container.CreateInstance(typeof(CustomerBLL));

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
        }

        [TestMethod]
        public void ExplicitConstructorInjectionTest()
        {
            _container.AddType(typeof(CustomerBLL));
            _container.AddType(typeof(Logger));
            _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBll = (CustomerBLL) _container.CreateInstance(typeof(CustomerBLL));

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
        }

        [TestMethod]
        public void GenericExplicitConstructorInjectionTest()
        {
            _container.AddType(typeof(CustomerBLL));
            _container.AddType(typeof(Logger));
            _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBll = _container.CreateInstance<CustomerBLL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
        }

        [TestMethod]
        public void GenericPropertiesInjectionTest()
        {
            _container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerCl = _container.CreateInstance<CustomerCL>();

            Assert.IsNotNull(customerCl);
            Assert.IsTrue(customerCl.GetType() == typeof(CustomerCL));
            Assert.IsNotNull(customerCl.CustomerDAL);
            Assert.IsNotNull(customerCl.CustomerDAL.GetType() == typeof(CustomerDAL));
            Assert.IsNotNull(customerCl.Logger);
            Assert.IsNotNull(customerCl.Logger.GetType() == typeof(Logger));
        }

        [TestMethod]
        public void PropertiesInjectionTest()
        {
            _container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBll = _container.CreateInstance<CustomerCL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerCL));
            Assert.IsNotNull(customerBll.CustomerDAL);
            Assert.IsNotNull(customerBll.CustomerDAL.GetType() == typeof(CustomerDAL));
            Assert.IsNotNull(customerBll.Logger);
            Assert.IsNotNull(customerBll.Logger.GetType() == typeof(Logger));
        }

        [TestMethod]
        public void ExplicitPropertiesInjectionTest()
        {
            _container.AddType(typeof(CustomerCL));
            _container.AddType(typeof(Logger));
            _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBll = (CustomerCL) _container.CreateInstance(typeof(CustomerCL));

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerCL));
            Assert.IsNotNull(customerBll.CustomerDAL);
            Assert.IsNotNull(customerBll.CustomerDAL.GetType() == typeof(CustomerDAL));
            Assert.IsNotNull(customerBll.Logger);
            Assert.IsNotNull(customerBll.Logger.GetType() == typeof(Logger));
        }

        [TestMethod]
        public void GenericExplicitPropertiesInjectionTest()
        {
            _container.AddType(typeof(CustomerCL));
            _container.AddType(typeof(Logger));
            _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerCl = _container.CreateInstance<CustomerCL>();

            Assert.IsNotNull(customerCl);
            Assert.IsTrue(customerCl.GetType() == typeof(CustomerCL));
            Assert.IsNotNull(customerCl.CustomerDAL);
            Assert.IsNotNull(customerCl.CustomerDAL.GetType() == typeof(CustomerDAL));
            Assert.IsNotNull(customerCl.Logger);
            Assert.IsNotNull(customerCl.Logger.GetType() == typeof(Logger));
        }
    }
}