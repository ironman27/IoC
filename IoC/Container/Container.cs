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
        private readonly Dictionary<Type, Type> _typesDictionary;

        public Container(ICreator creator)
        {
            _creator = creator;
            _typesDictionary = new Dictionary<Type, Type>();
        }

        public void AddType(Type type)
        {
            if (!_typesDictionary.ContainsKey(type))
                _typesDictionary.Add(type, type);
        }

        public void AddType(Type type, Type abstractType)
        {
            if (!_typesDictionary.ContainsKey(abstractType))
                _typesDictionary.Add(abstractType, type);
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
                        if (!_typesDictionary.ContainsKey(abstractType))
                            _typesDictionary.Add(abstractType, type);
                    }
                    else
                    {
                        if (!_typesDictionary.ContainsKey(type))
                            _typesDictionary.Add(type, type);
                    }
                }

                foreach (var constructorInfo in type.GetConstructors())
                {
                    if (constructorInfo.CustomAttributes.Any(c => c.AttributeType == typeof(ImportConstructorAttribute)))
                    {
                        if (!_typesDictionary.ContainsKey(type))
                            _typesDictionary.Add(type, type);
                    }
                }

                foreach (var memberInfo in type.GetMembers())
                {
                    if (memberInfo.CustomAttributes.Any(a => a.AttributeType == typeof(ImportAttribute)))
                    {
                        if (!_typesDictionary.ContainsKey(type))
                            _typesDictionary.Add(type, type);
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
            if (!_typesDictionary.ContainsKey(type))
            {
                throw new Exception($"Type {type.FullName} can not be created!");
            }

            Type specificType = GetSpecificType(type);
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

            if (specificType.GetCustomAttribute<ImportConstructorAttribute>() != null)
            {
                return instance;
            }

            var properties = specificType.GetProperties()
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(ImportAttribute)));

            foreach (var property in properties)
            {
                var propertyInstance = _creator.Create(GetSpecificType(property.PropertyType), CreateParameters(GetSpecificType(property.PropertyType)).ToArray());
                property.SetValue(instance, propertyInstance);
            }

            return instance;
        }

        private List<object> CreateParameters(Type type)
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

        private Type GetSpecificType(Type type)
        {
            if (_typesDictionary.ContainsKey(type))
            {
                return _typesDictionary[type];
            }

            return type;
        }
    }
}
