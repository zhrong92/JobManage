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

        public async Task<PageList<JobRunLog>> GetPagedListAsync(int pageIndex, int pageSize, string searchValue)
        {
            var count = await _repository.CountAsync(j => j.JobName.Contains(searchValue));
            var list = await _repository.GetPagedListAsync(pageIndex, pageSize, j => j.JobName.Contains(searchValue));
            var result = new PageList<JobRunLog>()
            {
                TotalCount = count,
                Datas = list
            };
            return result;
        }

        public async Task<JobRunLog> InsertAsync(JobRunLog log)
        {
            return await _repository.InsertAsync(log);
        }

    }
}
