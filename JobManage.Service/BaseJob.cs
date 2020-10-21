using Quartz;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Service
{
    public class BaseJob : IJob
    {
        private readonly IHttpClientFactory _clientFactory;
        public BaseJob(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var url = context.JobDetail.JobDataMap.GetString("RequestUrl");
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
            }
        }

    }
}
