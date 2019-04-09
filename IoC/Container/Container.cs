using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IoC.Container.Attributes;

namespace IoC.Container
{
    public class Container
    {
        private readonly ICreator _creator;
        public readonly Dictionary<Type, Type> TypesDictionary;

        public Container(ICreator creator)
        {
            _creator = creator;
            TypesDictionary = new Dictionary<Type, Type>();
        }

        public void AddType(Type type)
        {
            if (!TypesDictionary.ContainsKey(type))
                TypesDictionary.Add(type, type);
        }

        public void AddType(Type type, Type abstractType)
        {
            if (!TypesDictionary.ContainsKey(abstractType))
                TypesDictionary.Add(abstractType, type);
        }

        public void AddAssembly(Assembly assembly)
        {
            var types = assembly.ExportedTypes;
            foreach (var type in types)
            {
                var attribute = type.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(ExportAttribute));
                if (attribute != null)
                {
                    if (attribute.ConstructorArguments.Count > 0)
                    {
                        var exportAttribute = type.GetCustomAttribute<ExportAttribute>();
                        var abstractType = exportAttribute.AbstractType;
                        if (!TypesDictionary.ContainsKey(abstractType))
                            TypesDictionary.Add(abstractType, type);
                    }
                    else
                    {
                        if (!TypesDictionary.ContainsKey(type))
                            TypesDictionary.Add(type, type);
                    }
                }

                foreach (var constructorInfo in type.GetConstructors())
                {
                    if (constructorInfo.CustomAttributes.Any(c => c.AttributeType == typeof(ImportConstructorAttribute)))
                    {
                        if (!TypesDictionary.ContainsKey(type))
                            TypesDictionary.Add(type, type);
                    }
                }

                foreach (var memberInfo in type.GetMembers())
                {
                    if (memberInfo.CustomAttributes.Any(a => a.AttributeType == typeof(ImportAttribute)))
                    {
                        if (!TypesDictionary.ContainsKey(type))
                            TypesDictionary.Add(type, type);
                    }
                }
            }
        }

        public T CreateInstance<T>()
        {
            Type type = typeof(T);
            return (T)CreateInstance(type);
        }

        public object CreateInstance(Type type)
        {
            if (!TypesDictionary.ContainsKey(type))
            {
                throw new Exception($"Type {type.FullName} can not be created!");
            }

            Type specificType = GetSpecificType(type);

            //create constructor parameters, resolve dependencies
            if (!specificType.GetConstructors().Any())
            {
                throw new Exception($"Type {specificType.FullName} don`t have public constructors!");
            }
            ConstructorInfo constructorInfo = specificType.GetConstructors().FirstOrDefault();
            
            List<object> parametersList = new List<object>();
            if (constructorInfo != null)
            {
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    var specificParameterInfoType = GetSpecificType(parameterInfo.ParameterType);
                    parametersList.Add(_creator.Create(specificParameterInfoType, CreateParameters(specificParameterInfoType).ToArray()));
                }
            }

            var instance = _creator.Create(specificType, parametersList.ToArray());

            //create members, resolve dependencies
            var properties = specificType.GetProperties()
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(ImportAttribute)));

            foreach (var property in properties)
            {
                var propertyInstance = _creator.Create(GetSpecificType(property.PropertyType), CreateParameters(GetSpecificType(property.PropertyType)).ToArray());
                property.SetValue(instance, propertyInstance);
            }

            return instance;
        }

        public List<object> CreateParameters(Type type)
        {
            if (!type.GetConstructors().Any())
            {
                throw new Exception($"Type {type.FullName} don`t have public constructors!");
            }
            ConstructorInfo constructorInfo = type.GetConstructors().FirstOrDefault();
            List<object> parametersList = new List<object>();
            if (constructorInfo != null)
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    parametersList.Add(_creator.Create(GetSpecificType(parameterInfo.ParameterType), CreateParameters(parameterInfo.ParameterType).ToArray()));
                }

            return parametersList;
        }

        public Type GetSpecificType(Type type)
        {
            if (TypesDictionary.ContainsKey(type))
            {
                return TypesDictionary[type];
            }

            return type;
        }
    }
}
