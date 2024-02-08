using Newtonsoft.Json;
using System.Text;

namespace ConsoleContainer.Repositories.Serialization
{
    internal class JsonBinarySerializer : IBinarySerializer
    {
        private readonly bool formatJson;

        public JsonBinarySerializer(bool formatJson)
        {
            this.formatJson = formatJson;
        }

        public byte[] Serialize<T>(T value)
        {
            var config = JsonConvert.SerializeObject(value, formatJson ? Formatting.Indented : Formatting.None);
            return Encoding.UTF8.GetBytes(config);
        }

        public T? Deserialize<T>(byte[] data)
        {
            var stringData = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(stringData);
        }
    }
}
