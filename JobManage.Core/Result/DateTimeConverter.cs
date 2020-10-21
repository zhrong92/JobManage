using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobManage.Core
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => DateTime.Parse(reader.GetString());

        public override void Write(Utf8JsonWriter writer, DateTime dateTime, JsonSerializerOptions options) => writer.WriteStringValue(dateTime.ToString(DateTimeFormat));
    }
}
