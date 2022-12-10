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
        throw new NotImplementedException();
    }

    public TDependency Resolve<TDependency>(int id)
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
}