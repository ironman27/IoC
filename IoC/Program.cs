using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using IoC.Container;
using IoC.Examples;

namespace IoC
{
    class Program
    {
        [Container.Attributes.Import]
        public ICalculator Calculator { get; set; }

        static void Main(string[] args)
        {
            //Creator creator = new Creator();
            //var container = new IoC.Container.Container(creator);
            //container.AddAssembly(Assembly.GetExecutingAssembly());

            //container.AddType(typeof(CustomerBLL));
            //container.AddType(typeof(Logger));
            //container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            //var customerBLL = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
            //var customerBLLByT = container.CreateInstance<CustomerBLL>();

            //foreach (var type in container.typesDictionary)
            //{
            //    Console.WriteLine($"{type.Key.FullName} - {type.Value.FullName}");
            //}

            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            //catalog.Catalogs.Add(new DirectoryCatalog("C:\\Users\\SomeUser\\Documents\\Visual Studio 2010\\Projects\\SimpleCalculator3\\SimpleCalculator3\\Extensions"));


            //Create the CompositionContainer with the parts in the catalog
            Container.Container _container = new Container.Container(new Creator());

            //Fill the imports of this object
            try
            {
                //this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

            Program p = new Program(); //Composition is performed in the constructor
            String s;
            Console.WriteLine("Enter Command:");
            while (true)
            {
                s = Console.ReadLine();
                Console.WriteLine(p.Calculator.Calculate(s));
            }



            //Console.ReadKey();
        }
    }

}
