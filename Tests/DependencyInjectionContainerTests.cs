using DependencyInjectionContainer;
using NUnit.Framework;
using Moq;

namespace Tests;

public class DependencyInjectionContainerTests
{
    private DependenciesConfiguration _configuration;
    private DependencyProvider _provider;
    
    [SetUp]
    public void Setup()
    {
        _configuration = new DependenciesConfiguration();
    }

    [Test]
    public void CreateDependencyForInterface()
    {
        _configuration.Register<InputData.IService, InputData.Service1>();
        _provider = new DependencyProvider(_configuration);

        var actual = _provider.Resolve<InputData.IService>();
        
        Assert.IsNotNull(actual);
    }
    
    [Test]
    public void CreateDependencyForAbstractClass()
    {
        _configuration.Register<InputData.AbstractService, InputData.Service2>();
        _provider = new DependencyProvider(_configuration);

        var actual = _provider.Resolve<InputData.AbstractService>();
        
        Assert.IsNotNull(actual);
    }
}