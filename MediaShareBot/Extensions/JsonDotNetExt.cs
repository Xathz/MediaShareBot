using System.Linq;
using Newtonsoft.Json.Linq;

namespace MediaShareBot.Extensions {

    public static class JsonDotNetExt {

        public static T FindValueByKey<T>(this JObject jObject, string key) => jObject.Descendants()
             .Where(x => x is JObject)
             .Where(x => x[key] != null)
             .Select(x => x[key].ToObject<T>())
             .FirstOrDefault();

        public static T FindValueByParentAndKey<T>(this JObject jObject, string parent, string key) => jObject.Descendants()
             .Where(x => x is JObject)
             .Where(x => x[parent] != null)
             .Select(x => x[parent])
                 .Where(x => x[key] != null)
                 .Select(x => x[key].ToObject<T>())
                 .FirstOrDefault();

    }

}
