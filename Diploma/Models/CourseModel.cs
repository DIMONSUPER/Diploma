using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace Diploma.Models
{
    public class CourseModel : IDTOModel
    {
        #region -- IDTOModel implementaion --

        [PrimaryKey]
        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        #region -- Public properties --

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [Ignore]
        [JsonProperty("lessons")]
        public IEnumerable<int> LessonsIds { get; set; }

        [JsonProperty("teacher_id")]
        public int TeacherId { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("is_visible")]
        public bool IsVisible { get; set; }

        [Ignore]
        [JsonProperty("users")]
        public List<int> UsersIds { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("publishedAt")]
        public DateTime PublishedAt { get; set; }

        #endregion
    }
}
