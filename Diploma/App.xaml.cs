using System.Globalization;
using Acr.UserDialogs;
using Diploma.Resources.Strings;
using Diploma.Services.Authorization;
using Diploma.Services.Course;
using Diploma.Services.Mapper;
using Diploma.Services.Repository;
using Diploma.Services.Rest;
using Diploma.Services.Settings;
using Diploma.Services.Style;
using Diploma.Services.User;
using Diploma.ViewModels;
using Diploma.ViewModels.Modal;
using Diploma.ViewModels.Tabs;
using Diploma.Views;
using Diploma.Views.Modal;
using Diploma.Views.Tabs;
using Plugin.LocalNotification;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Diploma
{
    public partial class App : PrismApplication
    {
        public static T Resolve<T>() where T : class => App.Current?.Container?.Resolve<T>();

        public App() : this(null)
        {
        }

        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer)
        {
        }

        #region -- Overrides --

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            containerRegistry.RegisterInstance(NotificationCenter.Current);

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<SearchPage, SearchPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationsPage, NotificationsPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<EditProfilePage, EditProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<NewCoursePage, NewCoursePageViewModel>();
            containerRegistry.RegisterForNavigation<CoursePage, CoursePageViewModel>();
            containerRegistry.RegisterForNavigation<NewLessonPage, NewLessonPageViewModel>();
            containerRegistry.RegisterForNavigation<LessonPage, LessonPageViewModel>();

            containerRegistry.RegisterInstance<IMapperService>(Container.Resolve<MapperService>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRestService>(Container.Resolve<RestService>());
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<IStyleService>(Container.Resolve<StyleService>());
            containerRegistry.RegisterInstance<IUserService>(Container.Resolve<UserService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<ICoursesService>(Container.Resolve<CoursesService>());
            containerRegistry.RegisterInstance<Services.Notification.INotificationService>(Container.Resolve<Services.Notification.NotificationService>());
        }

        protected override async void OnInitialized()
        {
            var userSettings = Container.Resolve<ISettingsManager>().UserSettings;

            LocalizationResourceManager.Current.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = GetCultureInfoFromLanguage(userSettings.CoursesLanguage);
            LocalizationResourceManager.Current.PropertyChanged += (sender, e) => Strings.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(Strings.ResourceManager);

            InitializeComponent();

            Resolve<IStyleService>().ChangeThemeTo((OSAppTheme)userSettings.AppTheme);

            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{Constants.PageConstants.MainTabbedPage}");
        }

        #endregion

        #region -- Private helpers --

        private CultureInfo GetCultureInfoFromLanguage(string language)
        {
            return language switch
            {
                Constants.LanguageConstansts.English => new("en-US"),
                Constants.LanguageConstansts.Russian => new("ru-RU"),
                Constants.LanguageConstansts.Ukrainian => new("uk-UA"),
                _ => throw new System.NotImplementedException(),
            };
        }

        #endregion
    }
}
