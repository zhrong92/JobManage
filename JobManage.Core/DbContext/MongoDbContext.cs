using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace JobManage.Core
{
    public class MongoDbContext : IMongoDbContext
    {
        public IMongoDatabase Database { get; set; }

        public virtual void InitializeDatabase(IMongoDatabase database)
        {
            Database = database;
        }

        public virtual IMongoCollection<T> Collection<T>()
        {
            return Database.GetCollection<T>(GetCollectionName<T>());
        }

        public virtual IMongoQueryable<T> Queryable<T>()
        {
            return Collection<T>().AsQueryable();
        }

        protected virtual string GetCollectionName<T>()
        {
            return typeof(T).Name;
        }

    }
}
