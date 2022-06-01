using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Repository;
using Diploma.Services.Rest;
using Diploma.Services.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Internals;

namespace Diploma.Services.Course
{
    public class CoursesService : ICoursesService
    {
        private readonly IRestService _restService;
        private readonly IRepositoryService _repositoryService;
        private readonly IUserService _userService;

        public CoursesService(
            IRestService restService,
            IRepositoryService repositoryService,
            IUserService userService)
        {
            _restService = restService;
            _repositoryService = repositoryService;
            _userService = userService;
        }

        #region -- ICoursesService implementation --

        public Task<AOResult<IEnumerable<CourseModel>>> GetAllCoursesAsync()
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                var url = $"{Constants.BASE_URL}/courses";

                var response = await _restService.GetAsync<JToken>(url);
                var result = JTokenHelper.ParseFromJToken<CourseModel>(response);

                await _repositoryService.SaveOrUpdateRangeAsync(result);

                return result;
            });
        }

        #endregion

        #region -- Private helpers --


        #endregion

        public List<T> TakeJson<T>(JToken token) where T : IDTOModel, new()
        {
            var list = new List<T>();

            if (token.Value<object>("data") is JArray dataArray)
            {
                foreach (var data in dataArray)
                {
                    T res = new();

                    res.Id = data.Value<int>("id");
                    foreach (var property in res.GetType().GetProperties())
                    {
                        var jsonProperty = property.CustomAttributes?.FirstOrDefault(x => x.AttributeType == typeof(JsonPropertyAttribute));
                        var pName = jsonProperty.ConstructorArguments[0].Value;
                        var pType = property.PropertyType;

                        if (property.GetValue(res) is IEnumerable)
                        {
                            var type = property.PropertyType.GenericTypeArguments[0];
                            var method = typeof(CoursesService).GetMethod(nameof(CoursesService.TakeJson));
                            var generic = method.MakeGenericMethod(type);
                            var result = generic.Invoke(this, new object[] { data["attributes"] });
                            property.SetValue(res, result);
                        }
                        else if (property.GetValue(res) is T)
                        {

                        }
                        else if (data["attributes"].Value<object>(pName) is object obj)
                        {
                            property.SetValue(res, obj);
                        }
                    }

                    list.Add(res);
                }
            }

            return list;
        }
    }
}
