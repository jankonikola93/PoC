using System.Linq.Dynamic.Core.CustomTypeProviders;
using Newtonsoft.Json.Linq;

namespace DynamicObjectMapper
{
    [DynamicLinqType]
    public static class DynamicLinqExtensions
    {
        public static JToken SelectToken(this JToken jToken, string path)
        {
            return jToken.SelectToken(path);
        }
    }
}