using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Diploma.Models
{
    public class CarouselBindableModel : BindableBase
    {
        #region -- Public properties --

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ObservableCollection<CourseBindableModel> _items;
        public ObservableCollection<CourseBindableModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        #endregion
    }
}
