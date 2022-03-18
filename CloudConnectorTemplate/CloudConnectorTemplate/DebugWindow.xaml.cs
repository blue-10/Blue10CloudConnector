using System;
using System.Windows;
using PureManApplicationDeployment;

namespace CloudConnectorTemplate
{
    /// <summary>
    /// The DebugWindow is meant for debugging only,
    /// It is currently used to debug updating the application.
    /// </summary>
    public partial class DebugWindow : Window
    {
        private PureManClickOnce mClickOnce;

        public DebugWindow()
        {
            InitializeComponent();

            mClickOnce = new PureManClickOnce("storage-endpoint");
        }

        private void ButtonIsNetworkInstalled_OnClick(object pSender, RoutedEventArgs pEvent)
        {
            LabelIsNetworkInstalled.Content = mClickOnce.IsNetworkDeployment;
        }

        private async void ButtonLocalVersion_OnClick(object pSender, RoutedEventArgs pEvent)
        {
            try
            {
                LabelLocalVersion.Content = await mClickOnce.CurrentVersion();
            }
            catch (Exception fExp)
            {
                Console.WriteLine(fExp.Message);
            }
        }

        private async void ButtonGetServerVersion_OnClick(object pSender, RoutedEventArgs pEvent)
        {
            try
            {
                LabelServerVersion.Content = await mClickOnce.ServerVersion();
            }
            catch (Exception fExp)
            {
                Console.WriteLine(fExp.Message);
            }
        }

        private async void ButtonUpdateAvailable_OnClick(object pSender, RoutedEventArgs pEvent)
        {
            try
            {
                LabelUpdateAvailable.Content = await mClickOnce.UpdateAvailable();
            }
            catch (Exception fExp)
            {
                Console.WriteLine(fExp.Message);
            }
        }

        private async void ButtonUpdate_OnClick(object pSender, RoutedEventArgs pEvent)
        {
            try
            {
                var fUpdateResult = await mClickOnce.Update();
                LabelUpdate.Content = fUpdateResult;
                if (fUpdateResult)
                    Environment.Exit(0);
            }
            catch (Exception fExp)
            {
                Console.WriteLine(fExp.Message);
            }
        }

        private void ButtonDataDir_OnClick(object pSender, RoutedEventArgs pEvent)
        {
            LabelDataDir.Content = mClickOnce.DataDir;
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
