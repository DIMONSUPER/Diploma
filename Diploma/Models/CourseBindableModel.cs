using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Mvvm;

namespace Diploma.Models
{
    public class CourseBindableModel : BindableBase
    {
        #region -- Public properties --

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _rating;
        public int Rating
        {
            get => _rating;
            set => SetProperty(ref _rating, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        private ObservableCollection<int> _lessonsIds;
        public ObservableCollection<int> LessonsIds
        {
            get => _lessonsIds;
            set => SetProperty(ref _lessonsIds, value);
        }

        private ObservableCollection<LessonBindableModel> _lessons;
        public ObservableCollection<LessonBindableModel> Lessons
        {
            get => _lessons;
            set => SetProperty(ref _lessons, value);
        }

        private int _teacherId;
        public int TeacherId
        {
            get => _teacherId;
            set => SetProperty(ref _teacherId, value);
        }

        private UserBindableModel _teacher;
        public UserBindableModel Teacher
        {
            get => _teacher;
            set => SetProperty(ref _teacher, value);
        }

        private string _language;
        public string Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        private string _category;
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        private ObservableCollection<int> _usersIds;
        public ObservableCollection<int> UsersIds
        {
            get => _usersIds;
            set => SetProperty(ref _usersIds, value);
        }

        private ObservableCollection<UserBindableModel> _users;
        public ObservableCollection<UserBindableModel> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        private DateTime _updatedAt;
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => SetProperty(ref _updatedAt, value);
        }

        private DateTime _publishedAt;
        public DateTime PublishedAt
        {
            get => _publishedAt;
            set => SetProperty(ref _publishedAt, value);
        }

        private ICommand _tappedCommand;
        public ICommand TappedCommand
        {
            get => _tappedCommand;
            set => SetProperty(ref _tappedCommand, value);
        }

        #endregion
    }
}
