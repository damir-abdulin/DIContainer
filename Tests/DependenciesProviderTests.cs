using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DependencyInjectionContainer;
using NUnit.Framework;
using Moq;

namespace Tests;

public class DependencyInjectionContainerTests
{
    private DependenciesConfiguration _configuration = null!;
    private DependencyProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        _configuration = new DependenciesConfiguration();
    }

    [Test]
    public void CreateInvalidConfigurations()
    {
        _configuration.Register<IEnumerable<int>, int>();

        try
        {
            _provider = new DependencyProvider(_configuration);
        }
        catch (DependenciesProviderException)
        {
            Assert.Pass();
        }

        Assert.Fail("Incorrect work with invalid configuration");
    }
    
    [Test]
    public void CreateValidConfigurations()
    {
        _configuration.Register<InputData.IService, InputData.Service1>();

        _provider = new DependencyProvider(_configuration);
        
        Assert.IsNotNull(_provider, "Incorrect work with invalid configuration");
    }
    
    [Test]
    public void CreateValidConfigurationsWithObject()
    {
        _configuration.Register<object, InputData.Service1>();

        _provider = new DependencyProvider(_configuration);
        
        Assert.IsNotNull(_provider, "Incorrect work with invalid configuration");
    }
    
    [Test]
    public void CreateImplementationAsAbstractClass()
    {
        _configuration.Register<InputData.AbstractService, InputData.AbstractService>();

        try
        {
            _provider = new DependencyProvider(_configuration);
        }
        catch (DependenciesProviderException)
        {
            Assert.Pass();
        }

        Assert.Fail("Incorrect work with invalid configuration");
    }
    
    [Test]
    public void CreateImplementationAsInterface()
    {
        _configuration.Register<InputData.IService, InputData.IService>();

        try
        {
            _provider = new DependencyProvider(_configuration);
        }
        catch (DependenciesProviderException)
        {
            Assert.Pass();
        }

        Assert.Fail("Incorrect work with invalid configuration");
    }

}