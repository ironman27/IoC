using IoC.Container.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Container.Examples
{
    [ImportConstructor]
    public class CustomerBLL
    {
        public CustomerBLL(ICustomerDAL dal, Logger logger)
        { }

        [Import]
        public ICustomerDAL CustomerDAL { get; set; }
        [Import]
        public Logger logger { get; set; }

    }

    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL
    {

    }


    [Export]
    public class Logger
    {

    }
}
