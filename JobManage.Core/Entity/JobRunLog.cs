using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JobManage.Core
{
    public class JobRunLog : IMongoDbEntity
    {
        public JobRunLog(string group, string name, double runTime, DateTime startTime, bool succ, string ex)
        {
            JobGroup = group;
            JobName = name;
            RunTime = runTime;
            StartTime = startTime;
            Succ = succ;
            Exception = ex;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string JobGroup { get; set; }
        public string JobName { get; set; }
        public double RunTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartTime { get; set; }
        public bool Succ { get; set; }
        public string Exception { get; set; }
    }
}
