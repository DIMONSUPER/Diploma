using System.Collections.Generic;
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

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("part")]
        public int Part { get; set; }

        [JsonProperty("video_url")]
        public string VideoUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [Ignore]
        [JsonProperty("tasks")]
        public List<int> TaskIds { get; set; }

        #endregion
    }
}
