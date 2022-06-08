using System;

using Xamarin.Forms;

namespace Diploma.Views
{
    public class LessonPage : ContentPage
    {
        public LessonPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

