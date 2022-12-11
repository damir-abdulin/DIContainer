using System.Collections;
using System.Dynamic;
using System.Net.WebSockets;
using System.Reflection;

namespace DependencyInjectionContainer;

public class DependencyProvider
{
    private readonly DependenciesConfiguration _configuration;
    
    public DependencyProvider(DependenciesConfiguration configuration)
    {
        if (!IsValidConfig(configuration))
            throw new DependenciesProviderException("Not valid configuration");
            
        _configuration = configuration;
    }
    
    public TDependency Resolve<TDependency>()
    {
        return (TDependency)Resolve(typeof(TDependency));
    }
    
    public object Resolve(Type dependency)
    {
        try
        {
            if (dependency.IsGenericType && dependency.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return GetImplementationList(dependency);
            }
            
            var impls = _configuration.GetImplementationsDescriptions(dependency);
            var type = impls[0].ToType();
            var impl = CreateInstance(type);

            if (impl is null)
                throw new DependenciesProviderException("Couldn't create object");

            return impl;
        }
        catch (DependenciesConfigurationException ex)
        {
            throw new DependenciesProviderException("Couldn't get implementation", ex);
        }
    }

    public TDependency Resolve<TDependency>(Enum id)
    {
        throw new NotImplementedException();
    }

    private static bool IsValidConfig(DependenciesConfiguration configs)
    {
        var dependencies = configs.GetAllDependencies();
        
        return 
            !(from dependency in dependencies 
                from implementation in configs.GetImplementationsDescriptions(dependency) 
                where !IsValidImplementation(dependency, implementation.ToType()) select dependency).Any();
    }

    private static bool IsValidImplementation(Type dependency, Type implementation)
    {
        return !(implementation.IsAbstract || implementation.IsInterface)
               && implementation.IsAssignableTo(dependency);
    }

    private object? CreateInstance(Type type)
    {
        var ctor = GetConstructor(type);

        if (ctor is null) return Activator.CreateInstance(type);

        var parameters = ctor.GetParameters();
        var myParameters = new object[parameters.Length];
        var currParam = 0;
        
        foreach (var parameter in parameters)
        {
            myParameters[currParam] = Resolve(parameter.ParameterType);
            currParam += 1;
        }

        return ctor.Invoke(myParameters);
    }

    private ConstructorInfo? GetConstructor(Type type)
    {
        return type.GetConstructors()
            .FirstOrDefault(
                ctor => ctor
                    .GetParameters()
                    .All(parameter => _configuration.IsContainsDependency(parameter.ParameterType)));
    }

    private object GetImplementationList(Type dependency)
    {
        var genericType = dependency.GenericTypeArguments[0];
        var implementations = _configuration.GetImplementationsDescriptions(genericType);

        var genericListType = typeof(List<>).MakeGenericType(genericType);
        var list = Activator.CreateInstance(genericListType);

        if (list is null)
            throw new DependenciesProviderException("Couldn't create implementations list");

        var result = (IList)list;

        foreach (var obj in implementations.Select(impl => CreateInstance(impl.ToType())))
        {
            if (obj is null)
                throw new DependenciesProviderException("Couldn't create object");

            result.Add(obj);
        }

        return result;
    }
}