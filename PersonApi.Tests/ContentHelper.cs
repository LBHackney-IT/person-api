using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace PersonApi.Tests
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
        }
    }
}
