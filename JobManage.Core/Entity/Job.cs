using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManage.Core
{
    public class Job : IMongoDbEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string CronExpression { get; set; }
        public string Description { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? NextOpTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LastOpTime { get; set; }
        public string RequestUrl { get; set; }

    }
}
