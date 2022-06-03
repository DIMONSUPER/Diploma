using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services.Mapper;
using Diploma.Services.Repository;
using Diploma.Services.Rest;
using Diploma.Services.Settings;
using Diploma.Services.User;
using Newtonsoft.Json.Linq;

namespace Diploma.Services.Course
{
    public class CoursesService : ICoursesService
    {
        private readonly IRestService _restService;
        private readonly IRepositoryService _repositoryService;
        private readonly ISettingsManager _settingsManager;
        private readonly IMapperService _mapperService;
        private readonly IUserService _userService;

        public CoursesService(
            IRestService restService,
            IRepositoryService repositoryService,
            ISettingsManager settingsManager,
            IMapperService mapperService,
            IUserService userService)
        {
            _restService = restService;
            _repositoryService = repositoryService;
            _settingsManager = settingsManager;
            _mapperService = mapperService;
            _userService = userService;
        }

        #region -- ICoursesService implementation --

        public Task<AOResult<IEnumerable<CourseModel>>> GetAllCoursesAsync(bool visibleOnly = true)
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                var url = $"{Constants.BASE_URL}/courses";

                var response = await _restService.GetAsync<JToken>(url);

                return JTokenHelper.ParseFromJToken<CourseModel>(response).Where(x =>
                (!visibleOnly || x.IsVisible) && (x.Language == _settingsManager.UserSettings.CoursesLanguage));
            });
        }

        public async Task<IEnumerable<CourseBindableModel>> ConvertToBindableCourses(IEnumerable<CourseModel> courses)
        {
            var mappedCourses = (await _mapperService.MapRangeAsync<CourseBindableModel>(courses)).ToList();

            for (int i = 0; i < mappedCourses.Count; i++)
            {
                mappedCourses[i].Users = new(await GetUsersForCourseAsync(mappedCourses[i]));

                var teacherResponse = await _userService.GetUserByIdAsync(mappedCourses[i].TeacherId);
                if (teacherResponse.IsSuccess)
                {
                    mappedCourses[i].Teacher = await _mapperService.MapAsync<UserBindableModel>(teacherResponse.Result);
                }
            }

            return mappedCourses;
        }

        #endregion

        #region -- Private helpers --

        private async Task<IEnumerable<UserBindableModel>> GetUsersForCourseAsync(CourseBindableModel course)
        {
            var result = Enumerable.Empty<UserBindableModel>();

            var users = await _userService.GetAllUsersAsync();

            if (users.IsSuccess)
            {
                var neededUsers = users.Result.Where(x => course.UsersIds.Contains(x.Id));
                result = await _mapperService.MapRangeAsync<UserBindableModel>(neededUsers);
            }

            return result;
        }

        #endregion
    }
}
