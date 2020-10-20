using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace JobManage.Core
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }

        IMongoCollection<T> Collection<T>();

        IMongoQueryable<T> Queryable<T>();
    }
}
