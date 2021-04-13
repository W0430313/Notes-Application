using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        Package package;
        PackageId packageID;
        PackageVersion packageVersion;
        String appName;
        String publisher;
        String packageVersionString;
        public About()
        {
            this.InitializeComponent();

            package = Package.Current;
            packageID = package.Id;
            packageVersion = packageID.Version;
            appName = package.DisplayName;
            publisher = package.PublisherDisplayName;

            packageVersionString = string.Format("{0}.{1}.{2}.{3}",
                packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += About_BackRequested;
                
            
        }

        private async void About_BackRequested(object sender, BackRequestedEventArgs e)
        {
            try
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }

                e.Handled = true;
            }
            catch(Exception ex)
            {
                ContentDialog error = new ContentDialog { Title = "Error", Content = "There was an error going back, please try again", PrimaryButtonText = "OK" };
                await error.ShowAsync();
            }
        }
    }
}
