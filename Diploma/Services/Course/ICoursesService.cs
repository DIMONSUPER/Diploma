﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;

namespace Diploma.Services.Course
{
    public interface ICoursesService
    {
        Task<AOResult<IEnumerable<CourseModel>>> GetAllCoursesAsync(bool visibleOnly = true);

        Task<IEnumerable<CourseBindableModel>> ConvertToBindableCourses(IEnumerable<CourseModel> courses);

        Task<AOResult<CourseModel>> PostNewCourseAsync(CourseModel course);

        Task<AOResult<LessonModel>> PostNewLessonForCourseAsync(LessonBindableModel lessonModel, int courseId);
    }
}
