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
using Newtonsoft.Json;
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
                (x.TeacherId == _settingsManager.AuthorizationSettings.UserId || !visibleOnly || x.IsVisible)
                && (x.Language == _settingsManager.UserSettings.CoursesLanguage));
            });
        }

        public async Task<IEnumerable<CourseBindableModel>> ConvertToBindableCourses(IEnumerable<CourseModel> courses)
        {
            var mappedCourses = (await _mapperService.MapRangeAsync<CourseBindableModel>(courses)).ToList();

            for (int i = 0; i < mappedCourses.Count; i++)
            {
                mappedCourses[i].Users = new(await GetUsersForCourseAsync(mappedCourses[i]));
                mappedCourses[i].Lessons = new(await GetLessonsForCourseAsync(mappedCourses[i]));

                var teacherResponse = await _userService.GetUserByIdAsync(mappedCourses[i].TeacherId);
                if (teacherResponse.IsSuccess)
                {
                    mappedCourses[i].Teacher = await _mapperService.MapAsync<UserBindableModel>(teacherResponse.Result);
                }
            }

            return mappedCourses;
        }

        public Task<AOResult<CourseModel>> PostNewCourseAsync(CourseModel course)
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                var url = $"{Constants.BASE_URL}/courses";

                var data = new
                {
                    data = new
                    {
                        name = course.Name,
                        description = course.Description,
                        rating = 0,
                        category = course.Category,
                        image_url = course.ImageUrl,
                        users = course.UsersIds,
                        teacher_id = course.TeacherId,
                        language = course.Language,
                        is_visible = course.IsVisible,
                    }
                };

                var response = await _restService.PostAsync<JToken>(url, data);

                if (response is null)
                {
                    onFailure("Course is null");
                }

                return JTokenHelper.ParseFromJTokenSingle<CourseModel>(response);
            });
        }

        public Task<AOResult<LessonModel>> PostNewLessonForCourseAsync(LessonBindableModel lesson, int courseId)
        {
            return AOResult.ExecuteTaskAsync(async onFailure =>
            {
                var taskIds = (await Task.WhenAll(lesson.Tasks.Select(x => PostNewTaskAsync(x)))).Select(x => x.Id);

                lesson.TaskIds = new(taskIds);

                var lessonModel = await PostLessonForCourseAsync(courseId, lesson);

                await Task.WhenAll(taskIds.Select(x => PutLessonIdForTaskAsync(x, lessonModel.Id)));

                return lessonModel;
            });
        }

        #endregion

        #region -- Private helpers --

        private async Task<IEnumerable<LessonBindableModel>> GetLessonsForCourseAsync(CourseBindableModel course)
        {
            var result = new List<LessonBindableModel>();

            if (course.LessonsIds?.Any() ?? false)
            {
                var url = $"{Constants.BASE_URL}/lessons";

                var response = await _restService.GetAsync<JToken>(url);

                var lessonModels = JTokenHelper.ParseFromJToken<LessonModel>(response);

                result = (await _mapperService.MapRangeAsync<LessonBindableModel>(lessonModels)).OrderBy(x => x.Part).ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    result[i].Tasks = new(await GetTasksForLessonAsync(result[i]));
                }
            }

            return result;
        }

        private async Task<IEnumerable<TaskBindableModel>> GetTasksForLessonAsync(LessonBindableModel lesson)
        {
            var result = Enumerable.Empty<TaskBindableModel>();

            if (lesson.TaskIds?.Any() ?? false)
            {
                var url = $"{Constants.BASE_URL}/tasks";

                var response = await _restService.GetAsync<JToken>(url);

                var taskModels = JTokenHelper.ParseFromJToken<TaskModel>(response);

                result = await _mapperService.MapRangeAsync<TaskBindableModel>(taskModels);
            }

            return result;
        }

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

        private async Task<CourseModel> GetCourseByIdAsync(int id)
        {
            var url = $"{Constants.BASE_URL}/courses?filters[id][$eq]={id}";

            var response = await _restService.GetAsync<JToken>(url);

            return JTokenHelper.ParseFromJToken<CourseModel>(response)?.FirstOrDefault();
        }

        private async Task<TaskModel> PostNewTaskAsync(TaskBindableModel task)
        {
            var url = $"{Constants.BASE_URL}/tasks";

            var data = new
            {
                data = new
                {
                    question = task.Question,
                    answer = task.Answer,
                    type = task.Type,
                    possible_answers = task.PossibleAnswers,
                }
            };

            var response = await _restService.PostAsync<JToken>(url, data);
            var res = JTokenHelper.ParseFromJTokenSingle<TaskModel>(response);
            return res;
        }

        private async Task<TaskModel> PutLessonIdForTaskAsync(int taskId, int lessonId)
        {
            var url = $"{Constants.BASE_URL}/tasks/{taskId}";

            var data = new
            {
                data = new
                {
                    lesson_id = lessonId
                }
            };

            var response = await _restService.PutAsync<JToken>(url, data);

            return JTokenHelper.ParseFromJTokenSingle<TaskModel>(response);
        }

        private async Task<LessonModel> PostLessonAsync(LessonBindableModel lessonModel)
        {
            var url = $"{Constants.BASE_URL}/lessons";

            var data = new
            {
                data = new
                {
                    title = lessonModel.Title,
                    part = lessonModel.Part,
                    video_url = lessonModel.VideoUrl,
                    description = lessonModel.Description,
                    tasks = lessonModel.TaskIds,
                }
            };

            var response = await _restService.PostAsync<JToken>(url, data);

            return JTokenHelper.ParseFromJTokenSingle<LessonModel>(response);
        }

        private async Task<LessonModel> PostLessonForCourseAsync(int courseId, LessonBindableModel lessonModel)
        {
            var postedLesson = await PostLessonAsync(lessonModel);

            var currentCourse = await GetCourseByIdAsync(courseId);

            var lessonIds = currentCourse.LessonsIds?.ToList() ?? new List<int>();
            lessonIds.Add(postedLesson.Id);

            var url = $"{Constants.BASE_URL}/courses/{courseId}";

            var data = new
            {
                data = new
                {
                    lessons = lessonIds,
                }
            };

            var response = await _restService.PutAsync<CourseModel>(url, data);

            return postedLesson;
        }

        #endregion
    }
}
