using System.Diagnostics;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace Spt.Frontend;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        CheckForWebview();
        base.OnStartup(e);
    }

    /// <summary>
    ///     Yoinked from AcidPhantasm https://github.com/acidphantasm/acidphantasm-apbs/blob/main/ConfigApp/App.xaml.cs
    /// </summary>
    private void CheckForWebview()
    {
        try
        {
            CoreWebView2Environment.GetAvailableBrowserVersionString();
        }
        catch (Exception e)
        {
            var messageBoxTitle = "Missing Webview2 Runtime";
            var messageBoxMessage = "You are missing the Evergreen Bootstrapper, which is required to use WebView2 applications. \n\n Please download and install the Evergreen Bootstrapper or Standalone Installer (if offline). \n\n https://developer.microsoft.com/en-us/microsoft-edge/webview2";
            var messageBoxButtons = MessageBoxButton.OKCancel;

            // Let the user decide if the app should die or not (if applicable).
            if (MessageBox.Show(messageBoxMessage, messageBoxTitle, messageBoxButtons) == MessageBoxResult.OK)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://developer.microsoft.com/en-us/microsoft-edge/webview2",
                    UseShellExecute = true
                });
            }
            else
            {
                Current.Shutdown();
            }
        }
    }
}
