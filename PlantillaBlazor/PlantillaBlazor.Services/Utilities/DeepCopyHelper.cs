using Newtonsoft.Json;

namespace PlantillaBlazor.Services.Utilities
{
    public class DeepCopyHelper
    {
        public static T DeepCopy<T>(T obj)
        {
            if (obj == null)
                return default;


            string json = JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
