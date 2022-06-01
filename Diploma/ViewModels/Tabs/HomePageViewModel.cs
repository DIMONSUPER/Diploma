using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Diploma.Models;
using Diploma.Services.Authorization;
using Diploma.Services.Course;
using Diploma.Services.Rest;
using Diploma.Services.User;
using Prism.Navigation;

namespace Diploma.ViewModels.Tabs
{
    public class HomePageViewModel : BaseTabViewModel
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICoursesService _coursesService;

        public HomePageViewModel(
            INavigationService navigationService,
            IUserService userService,
            IAuthorizationService authorizationService,
            ICoursesService coursesService)
            : base(navigationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _coursesService = coursesService;
        }

        #region -- Public properties --

        private ObservableCollection<CarouselBindableModel> _homeItems;
        public ObservableCollection<CarouselBindableModel> HomeItems
        {
            get => _homeItems;
            set => SetProperty(ref _homeItems, value);
        }

        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await InitMockedNewsItemsAsync();
        }

        #endregion

        #region -- Private helpers --

        private async Task UpdateCoursesAsync()
        {
            var courses = await _coursesService.GetAllCoursesAsync();


        }

        private async Task InitMockedNewsItemsAsync()
        {
            await Task.Delay(200);

            HomeItems = new()
            {
                new()
                {
                    Items = new()
                    {
                        new() { Title = "Linear Algebra I: Linear Equations", ImagePath = "https://prod-discovery.edx-cdn.org/media/course/image/00a8431b-01a8-4f69-83b8-eab4785a71f8-a4819326bac8.small.jpeg" },
                        new() { Title = "Linear Algebra II: Matrix Algebra", ImagePath = "https://prod-discovery.edx-cdn.org/media/course/image/00a8431b-01a8-4f69-83b8-eab4785a71f8-a4819326bac8.small.jpeg" },
                        new() { Title = "Linear Algebra III: Determinants and Eigenvalues", ImagePath = "https://prod-discovery.edx-cdn.org/media/course/image/00a8431b-01a8-4f69-83b8-eab4785a71f8-a4819326bac8.small.jpeg" },
                        new() { Title = "Applications of Linear Algebra", ImagePath = "https://prod-discovery.edx-cdn.org/media/course/image/00a8431b-01a8-4f69-83b8-eab4785a71f8-a4819326bac8.small.jpeg" },
                        new() { Title = "Mathematical Optimization for Engineers", ImagePath = "https://prod-discovery.edx-cdn.org/media/course/image/00a8431b-01a8-4f69-83b8-eab4785a71f8-a4819326bac8.small.jpeg" },
                    },
                    Title = "Math",
                },
                new()
                {
                    Items = new()
                    {
                        new() { Title = "Learn to Program: The Fundamentals", ImagePath = "https://d3njjcbhbojbot.cloudfront.net/api/utilities/v1/imageproxy/https://d15cw65ipctsrr.cloudfront.net/18/2aa16c328a457cb910aa933bf2cd87/Professional-Certificate-Cloud-App.jpg?auto=format%2Ccompress&dpr=2&w=250&h=100&q=25&fit=fill&bg=FFF" },
                        new() { Title = "Python for Everybody", ImagePath = "https://d3njjcbhbojbot.cloudfront.net/api/utilities/v1/imageproxy/https://d15cw65ipctsrr.cloudfront.net/fe/163a0249a146fdab429b3908e28422/C4E-logo-spec.png?auto=format%2Ccompress&dpr=2&w=250&h=100&fit=fill&bg=FFF&q=25" },
                        new() { Title = "Python for Everybody", ImagePath = "https://d3njjcbhbojbot.cloudfront.net/api/utilities/v1/imageproxy/https://d15cw65ipctsrr.cloudfront.net/fe/163a0249a146fdab429b3908e28422/C4E-logo-spec.png?auto=format%2Ccompress&dpr=2&w=250&h=100&fit=fill&bg=FFF&q=25" },
                        new() { Title = "Python for Everybody", ImagePath = "https://d3njjcbhbojbot.cloudfront.net/api/utilities/v1/imageproxy/https://d15cw65ipctsrr.cloudfront.net/fe/163a0249a146fdab429b3908e28422/C4E-logo-spec.png?auto=format%2Ccompress&dpr=2&w=250&h=100&fit=fill&bg=FFF&q=25" },
                        new() { Title = "Python for Everybody", ImagePath = "https://d3njjcbhbojbot.cloudfront.net/api/utilities/v1/imageproxy/https://d15cw65ipctsrr.cloudfront.net/fe/163a0249a146fdab429b3908e28422/C4E-logo-spec.png?auto=format%2Ccompress&dpr=2&w=250&h=100&fit=fill&bg=FFF&q=25" },
                        new() { Title = "Python for Everybody", ImagePath = "https://d3njjcbhbojbot.cloudfront.net/api/utilities/v1/imageproxy/https://d15cw65ipctsrr.cloudfront.net/fe/163a0249a146fdab429b3908e28422/C4E-logo-spec.png?auto=format%2Ccompress&dpr=2&w=250&h=100&fit=fill&bg=FFF&q=25" },
                    },
                    Title = "Programming",
                },
            };
        }

        #endregion
    }
}
