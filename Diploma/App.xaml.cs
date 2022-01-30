using Prism;
using Prism.Ioc;
using Prism.Unity;
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

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync(Constants.PageConstants.MainPage);
        }

        #endregion
    }
}
