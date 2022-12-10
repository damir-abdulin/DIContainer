using System.Collections.Concurrent;

namespace DependencyInjectionContainer;

public class DependenciesConfiguration
{
    private readonly ConcurrentDictionary<Type, ConcurrentBag<ImplementationDescription>> _lookupTable;

    public DependenciesConfiguration()
    {
        _lookupTable = new ConcurrentDictionary<Type, ConcurrentBag<ImplementationDescription>>();
    }

    public void Register<TDependency, TImplementation>(Enum? id = null, Lifecycle lifecycle = Lifecycle.Transient)
    {
        var typeDependency = typeof(TDependency);
        if (typeDependency == null) 
            throw new DependenciesConfigurationException($"typeof({nameof(typeDependency)}) returns null");
        
        var typeImplementation = typeof(TImplementation);
        if (typeImplementation == null)
            throw new DependenciesConfigurationException($"typeof({nameof(typeImplementation)}) returns null");

        _lookupTable.TryAdd(typeDependency, new ConcurrentBag<ImplementationDescription>());
        
        _lookupTable[typeDependency].Add(new ImplementationDescription(id, typeImplementation, lifecycle));
    }

    public List<Type> GetImplementations<TDependency>()
    {
        return GetImplementations(typeof(TDependency));
    }
    
    public List<Type> GetImplementations(Type dependency)
    {
        if (!_lookupTable.TryGetValue(dependency, out var implDescriptions))
            throw new DependenciesConfigurationException(
                $"Configuration hasn't implementation for {nameof(dependency)}");
        
        return implDescriptions.Select(des => des.ToType()).ToList();
    }

    public Type GetImplementation<TDependency>(Enum id)
    {
        return GetImplementation(typeof(TDependency), id);
    }
    public Type GetImplementation(Type dependency, Enum id)
    {
        if (!_lookupTable.TryGetValue(dependency, out var implDescriptions))
            throw new DependenciesConfigurationException(
                $"Configuration hasn't implementation for {nameof(dependency)}");

        try
        {
            return implDescriptions.First(des => des.Id != null && des.Id.Equals(id)).ToType();
        }
        catch (InvalidOperationException)
        {
            throw new DependenciesConfigurationException(
                $"Configuration hasn't implementation for {nameof(dependency)} with id {id}");
        }
    }
}