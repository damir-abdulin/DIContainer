namespace DependencyInjectionContainer;

public class DependencyProvider
{
    private DependenciesConfiguration _dependencies;
    
    public DependencyProvider(DependenciesConfiguration dependencies)
    {
        _dependencies = dependencies;
    }

    public TDependency Resolve<TDependency>()
    {
        throw new NotImplementedException();
    }

    public TDependency Resolve<TDependency>(int id)
    {
        throw new NotImplementedException();
    }
    
}