using System.Collections.Concurrent;

namespace DependencyInjectionContainer;

public class DependenciesConfiguration
{
    private readonly ConcurrentDictionary<Type, ConcurrentBag<Type>> _lookupTable;

    public DependenciesConfiguration()
    {
        _lookupTable = new ConcurrentDictionary<Type, ConcurrentBag<Type>>();
    }

    public void Register<TDependency, TImplementation>()
    {
        var typeDependency = typeof(TDependency);
        if (typeDependency == null) 
            throw new DependenciesConfigurationException($"typeof({nameof(typeDependency)}) returns null");
        
        var typeImplementation = typeof(TImplementation);
        if (typeImplementation == null)
            throw new DependenciesConfigurationException($"typeof({nameof(typeImplementation)}) returns null");

        _lookupTable.TryAdd(typeDependency, new ConcurrentBag<Type>());
        
        _lookupTable[typeDependency].Add(typeImplementation);
    }
    
    public void Register<TDependency, TImplementation>(int id)
    {
        throw new NotImplementedException();
    }

    public ConcurrentBag<Type> GetImplementations(Type dependency)
    {
        if (!_lookupTable.TryGetValue(dependency, out var impl))
            throw new DependenciesConfigurationException(
                $"Configuration hasn't implementation for {nameof(dependency)}");
        
        return impl;
    }
}