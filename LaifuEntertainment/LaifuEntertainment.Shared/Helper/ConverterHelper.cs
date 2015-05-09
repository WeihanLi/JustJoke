using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace LaifuEntertainment.Helper
{
    public class ConverterHelper
    {
        public static T JsonToObj<T>(string jsonData) where T : class
        {
            T obj;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData)))
            {
                obj = serializer.ReadObject(stream) as T;
            }
            return obj;
        }
    }
}
