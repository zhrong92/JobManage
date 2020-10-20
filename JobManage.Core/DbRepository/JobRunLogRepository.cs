using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Core
{
    public class JobRunLogRepository
    {
        private readonly MongoDbRepository<JobRunLog> _repository;

        public JobRunLogRepository(MongoDbRepository<JobRunLog> repository)
        {
            _repository = repository;
        }

        public async Task<List<JobRunLog>> GetPagedListAsync(int skipCount, int returnCount, string searchValue, bool succ)
        {
            return await _repository.GetPagedListAsync(skipCount, returnCount, j => j.JobName.Contains(searchValue) && j.Succ == succ);
        }

        public async Task<int> CountAsync(string searchValue, bool succ)
        {
            return await _repository.CountAsync(j => j.JobName.Contains(searchValue) && j.Succ == succ);
        }

        public async Task<JobRunLog> InsertAsync(JobRunLog log)
        {
            return await _repository.InsertAsync(log);
        }

    }
}
