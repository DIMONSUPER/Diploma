using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Diploma.Models;
using Prism.Navigation;

namespace Diploma.ViewModels.Tabs
{
    public class HomePageViewModel : BaseTabViewModel
    {
        private const string MOCKED_URL = @"http://fpm.dnu.dp.ua/wp-content/uploads/2022/01/pexels-photo-1595391-1880x750.jpeg";
        private const string MOCKED_TITLE = @"Підготовка для складання Єдиного вступного іспиту з іноземної мови (ЄВІ) для вступу до магістратури";

        public HomePageViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
        }

        #region -- Public properties --

        private ObservableCollection<NewsModel> _newsItems;
        public ObservableCollection<NewsModel> NewsItems
        {
            get => _newsItems;
            set => SetProperty(ref _newsItems, value);
        }

        #endregion

        #region -- Overrides --

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await InitMockedNewsItemsAsync();
        }

        #endregion

        #region -- Private helpers --

        private async Task InitMockedNewsItemsAsync()
        {
            await Task.Delay(200);

            NewsItems = new()
            {
                new() { Title = MOCKED_TITLE, ImageUrl = MOCKED_URL, CommentsCount = 2, PublicationDate = DateTime.Now },
                new() { Title = MOCKED_TITLE, ImageUrl = MOCKED_URL, CommentsCount = 2, PublicationDate = DateTime.Now },
                new() { Title = MOCKED_TITLE, ImageUrl = MOCKED_URL, CommentsCount = 2, PublicationDate = DateTime.Now },
                new() { Title = MOCKED_TITLE, ImageUrl = MOCKED_URL, CommentsCount = 2, PublicationDate = DateTime.Now },
                new() { Title = MOCKED_TITLE, ImageUrl = MOCKED_URL, CommentsCount = 2, PublicationDate = DateTime.Now },
                new() { Title = MOCKED_TITLE, ImageUrl = MOCKED_URL, CommentsCount = 2, PublicationDate = DateTime.Now },
            };
        }

        #endregion
    }
}
