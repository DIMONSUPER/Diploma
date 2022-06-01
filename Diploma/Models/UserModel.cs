using System;
using Newtonsoft.Json;
using SQLite;

namespace Diploma.Models
{
    public class UserModel : IDTOModel
    {
        #region -- IDTOModel implementaion --

        [PrimaryKey]
        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        #region -- Public properties --

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("confirmed")]
        public bool IsConfirmed { get; set; }

        [JsonProperty("blocked")]
        public bool IsBlocked { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("role_id")]
        public int RoleId { get; set; }

        #endregion
    }
}
