using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace toofz.NecroDancer.Web.Api.Tests
{
    internal static class SerializationHelper
    {
        public static string Serialize<T>(MediaTypeFormatter formatter, T value)
        {
            // Create a dummy HTTP Content.
            Stream stream = new MemoryStream();
            var content = new StreamContent(stream);
            /// Serialize the object.
            formatter.WriteToStreamAsync(typeof(T), value, stream, content, null).Wait();
            // Read the serialized string.
            stream.Position = 0;
            return content.ReadAsStringAsync().Result;
        }

        public static T Deserialize<T>(MediaTypeFormatter formatter, string str) where T : class
        {
            // Write the serialized string to a memory stream.
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            // Deserialize to an object of type T
            return formatter.ReadFromStreamAsync(typeof(T), stream, null, null).Result as T;
        }
    }
}
