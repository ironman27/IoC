using System;

namespace IoC.Container
{
    public interface ICreator
    {
        object Create(Type type, object[] parameters);
    }
}
