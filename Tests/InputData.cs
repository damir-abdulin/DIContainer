namespace Tests;

public class InputData
{
    public enum ServiceImplementations : int
    {
        First,
        Second
    }
    
    public interface IService { }
    public abstract class AbstractService { }
    
    
    public class Service1 : AbstractService, IService { }
    public class Service2 : AbstractService, IService { }

    public class ServiceImpl : IService
    {
        public ServiceImpl(IRepository repository) { }
    }

    public interface IRepository { }

    public class RepositoryImpl : IRepository
    {
        public RepositoryImpl() {}
    }

}