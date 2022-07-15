using System;
using Newtonsoft.Json;
using SQLite;

namespace Diploma.Models
{
    public class NotificationModel : IDTOModel
    {
        #region -- IDTOModel implementaion --

        [PrimaryKey]
        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
