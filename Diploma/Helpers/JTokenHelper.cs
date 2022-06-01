using System.Collections.Generic;
using Diploma.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Helpers
{
    public static class JTokenHelper
    {
        public static IEnumerable<T> ParseFromJToken<T>(JToken token) where T : IDTOModel
        {
            List<T> result = new();

            foreach (var data in token["data"])
            {
                var model = JsonSerializer.Create().Deserialize<T>(data["attributes"].CreateReader());
                model.Id = data.Value<int>("id");
                result.Add(model);
            }

            return result;
        }
    }
}
