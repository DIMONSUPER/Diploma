using System.Threading.Tasks;
using Diploma.Resources.Strings;
using Diploma.Services.Rest;
using Diploma.Services.Settings;
using Diploma.Services.Style;
using Diploma.Views;
using Diploma.Views.Tabs;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
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
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<NewsPage>();

            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRestService>(Container.Resolve<RestService>());
            containerRegistry.RegisterInstance<IStyleService>(Container.Resolve<StyleService>());
        }

        protected override async void OnInitialized()
        {
            LocalizationResourceManager.Current.PropertyChanged += (sender, e) => Strings.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(Strings.ResourceManager);

            InitializeComponent();
            Resolve<IStyleService>().ChangeThemeTo(OSAppTheme.Dark);
            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{Constants.PageConstants.MainTabbedPage}");
        }

        #endregion
    }
}
