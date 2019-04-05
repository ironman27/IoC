using IoC.Container.Examples;
using IoC.DependencyInjectionSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoC
{
    class Program
    {
        static void Main(string[] args)
        {
            //IDoubleOperation doubleOperation = DoubleOperation.Instance;
            //Console.WriteLine("Result of double operation = " +
            //doubleOperation.PerformDoubleOperation(3, 5));
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadLine();

            var container = new IoC.Container.Container();
            container.AddType(typeof(CustomerBLL));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBLL = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
            var customerBLLByT = container.CreateInstance<CustomerBLL>();
        }
    }

}
