using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace Diploma.Models
{
    public class TaskModel : IDTOModel
    {
        #region -- IDTOModel implementaion --

        [PrimaryKey]
        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        #region -- Public properties --

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [Ignore]
        [JsonProperty("possible_answers")]
        public List<string> PossibleAnswers { get; set; }

        [JsonProperty("lesson_id")]
        public int LessonId { get; set; }

        #endregion
    }
}
