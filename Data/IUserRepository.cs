namespace DotNetAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void RemoveEntity<T>(T entityToRemove);
        public void AddEntity<T>(T entityToAdd);
    }
}