using FFImageLoading.Forms.Platform;
using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;

namespace Diploma.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        #region -- Overrides --

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            CachedImageRenderer.Init();

            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }

        #endregion

        internal class iOSInitializer : IPlatformInitializer
        {
            #region -- IPlatformInitializer implementation --

            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
            }

            #endregion
        }
    }
}
