using System.Collections.Generic;
using System.Threading.Tasks;
using Diploma.Helpers;
using Diploma.Models;

namespace Diploma.Services.Course
{
    public interface ICoursesService
    {
        Task<AOResult<IEnumerable<CourseModel>>> GetAllCoursesAsync();
    }
}
