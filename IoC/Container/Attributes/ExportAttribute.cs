using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Container.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ExportAttribute : Attribute
    {
        public Type AbstractType; 

        public ExportAttribute() { }

        public ExportAttribute(Type t)
        {
            AbstractType = t;
        }
    }
}
