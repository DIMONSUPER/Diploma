using System;
using Prism.Mvvm;

namespace Diploma.Models
{
    public class NewsBindableModel : BindableBase
    {
        #region -- Public properties --

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private DateTime _publicationDate;
        public DateTime PublicationDate
        {
            get => _publicationDate;
            set => SetProperty(ref _publicationDate, value);
        }

        private int _commentsCount;
        public int CommentsCount
        {
            get => _commentsCount;
            set => SetProperty(ref _commentsCount, value);
        }

        #endregion
    }
}
