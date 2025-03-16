namespace CodeBase.Infrastructure.SaveService
{
    public interface ISaveHandler
    {
        public void SaveFromObject(ISaver saver);
    }
}