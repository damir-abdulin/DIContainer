using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DependencyInjectionContainer;
using NUnit.Framework;

namespace Tests;

public class DependenciesConfigurationTests
{
    private DependenciesConfiguration _configuration = null!;

    [SetUp]
    public void Setup()
    {
        _configuration = new DependenciesConfiguration();
    }

    [Test]
    public void GetRealizedDependency()
    {
        _configuration.Register<IEnumerable<int>, List<int>>();
        _configuration.Register<IEnumerable<int>, ConcurrentBag<int>>();

        var actual = _configuration.GetImplementations<IEnumerable<int>>();

        var isContainsAll = actual.Contains(typeof(List<int>))
                            && actual.Contains(typeof(ConcurrentBag<int>));

        Assert.IsTrue(isContainsAll, $"GetImplementations returns invalid value");
    }

    [Test]
    public void GetNotRealizedDependency()
    {
        try
        {
            _configuration.GetImplementations<int>();
        }
        catch (DependenciesConfigurationException e)
        {
            Assert.Pass();
        }

        Assert.Fail("GetImplementation<int>() doesn't return DependenciesConfigurationException");
    }

    [Test]
    public void GetDependencyWithId()
    {
        _configuration.Register<InputData.IService, InputData.Service1>(InputData.ServiceImplementations.First);
        _configuration.Register<InputData.IService, InputData.Service2>(InputData.ServiceImplementations.Second);

        var actual = _configuration.GetImplementation<InputData.IService>(InputData.ServiceImplementations.Second);

        var excepted = typeof(InputData.Service2);

        Assert.AreEqual(excepted, actual, "GetImplementation returns invalid value");
    }
    
    [Test]
    public void GetDependencyWithNotExistedId()
    {
        _configuration.Register<InputData.IService, InputData.Service1>(InputData.ServiceImplementations.First);

        try
        {
            _configuration.GetImplementation<InputData.IService>(InputData.ServiceImplementations.Second);
        }
        catch (DependenciesConfigurationException)
        {
            Assert.Pass();
        }
        
        Assert.Fail("GetImplementation returns invalid value");
    }
}