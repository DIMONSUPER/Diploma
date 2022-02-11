using System;

namespace Diploma.Models
{
    public class NewsModel
    {
        #region -- Public properties --

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublicationDate { get; set; }

        public int CommentsCount { get; set; }

        #endregion
    }
}
