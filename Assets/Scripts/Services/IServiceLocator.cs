namespace Infrastructure.Services
{
    public interface IServiceLocator
    {
        T Get<T>();
        void Reg<T>(T service) where T : IService;
    }
}