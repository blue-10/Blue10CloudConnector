using System;
using System.Deployment.Application;
using System.Windows;

namespace CloudConnectorTemplate_DNF
{
    /// <summary>
    /// The DebugWindow is meant for debugging only,
    /// It is currently used to debug updating the application.
    /// </summary>
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            Left = SystemParameters.PrimaryScreenWidth - 410;
            Top = SystemParameters.PrimaryScreenHeight - 290;
            InitializeComponent();
        }

        public void CheckForUpdate()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var fAppDeploy = ApplicationDeployment.CurrentDeployment;

                try
                {
                    var fCheck = fAppDeploy.CheckForUpdate();
                    if (fCheck)
                        Update(fAppDeploy);
                }
                catch (Exception fException)
                {
                    MessageBox.Show(fException.Message);
                }
            }
        }

        private void Update(ApplicationDeployment pAppDeploy)
        {
            try
            {
                var fResult = pAppDeploy.Update();
                if (fResult)
                {
                    System.Windows.Forms.Application.Restart();
                    Environment.Exit(0);
                }

            }
            catch (Exception fException)
            {
                MessageBox.Show(fException.Message);
            }
        }

        private void Window_Loaded(object pSender, RoutedEventArgs pEvent)
        {
            var fIsNetworkDeployed = ApplicationDeployment.IsNetworkDeployed;

            NetworkDeployedLabel.Content = $"IsNetworkDeployed: { fIsNetworkDeployed }";

            if (fIsNetworkDeployed)
                CurrentVersionLabel.Content = $"Current Version { ApplicationDeployment.CurrentDeployment.CurrentVersion }";
        }

    }
}
