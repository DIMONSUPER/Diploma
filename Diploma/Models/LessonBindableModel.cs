using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Mvvm;

namespace Diploma.Models
{
    public class LessonBindableModel : BindableBase
    {
        #region -- Public properties --

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private int _part;
        public int Part
        {
            get => _part;
            set => SetProperty(ref _part, value);
        }

        private string _videoUrl;
        public string VideoUrl
        {
            get => _videoUrl;
            set => SetProperty(ref _videoUrl, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private ObservableCollection<int> _taskIds;
        public ObservableCollection<int> TaskIds
        {
            get => _taskIds;
            set => SetProperty(ref _taskIds, value);
        }

        private ObservableCollection<TaskBindableModel> _tasks;
        public ObservableCollection<TaskBindableModel> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
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
