using Newtonsoft.Json;
using SQLite;

namespace Diploma.Models
{
    public class LessonModel : IDTOModel
    {
        #region -- IDTOModel implementaion --

        [PrimaryKey]
        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        #region -- Public properties --


        #endregion
    }
}
