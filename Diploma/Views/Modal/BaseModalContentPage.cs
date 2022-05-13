using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Diploma.Views.Modal
{
    public class BaseModalContentPage : BaseContentPage
    {
        public BaseModalContentPage()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.OverFullScreen);
        }
    }
}
