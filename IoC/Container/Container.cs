using IoC.Container.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Container
{
    public class Container
    {
        Dictionary<Type, Type> typeDictionary = new Dictionary<Type, Type>();

        public Container(){}

        public void AddType(Type type)
        {
            typeDictionary.Add(type, type);
        }

        public void AddType(Type type, Type interfaceType)
        {
            typeDictionary.Add(type, interfaceType);
        }

        public void AddAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                type.
            }
        }

        public CustomerBLL CreateInstance(Type type)
        {
            throw new NotImplementedException();
        }

        public object CreateInstance<T>()
        {
            throw new NotImplementedException();
        }
    }
}
