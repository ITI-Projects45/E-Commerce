using E_Commerce.DB;

namespace E_Commerce.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataBaseContext _DataBaseContext;
        public Repository(DataBaseContext DataBaseContext)
        {

            _DataBaseContext = DataBaseContext;
        }

        public void Create(T entity)
        {
            _DataBaseContext.Set<T>().Add(entity);
        }

        public void Delete(int id)
        {
            T entity = GetById(id);
            if (entity != null)
            {
                _DataBaseContext.Set<T>().Remove(entity);
            }
        }

        public IQueryable<T> GetAll(int page = 1)
        {
            return _DataBaseContext.Set<T>().Skip((page-1) * 10).Take(10);
        }

        public T GetById(int id)
        {
            return _DataBaseContext.Set<T>().Find(id);
        }

        public void Save()
        {
            _DataBaseContext.SaveChanges();
        }

        public void Update(T entity)
        {

            _DataBaseContext.Set<T>().Update(entity);

        }
    }
}
