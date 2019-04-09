using System;

namespace IoC.Container
{
    public class Creator : ICreator
    {
        public object Create(Type type, object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }
    }
}
