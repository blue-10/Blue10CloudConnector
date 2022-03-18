using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CloudConnectorTemplate.Interface;
using CloudConnectorTemplate.Interfaces;
using CloudConnectorTemplate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConnectorTemplate
{
    public partial class MainWindow : Window
    {
        private ServiceProvider? mServiceProvider;
        private readonly Timer? mTimer;
        private readonly Icon mRed;
        private readonly Icon mDefault;
        private DebugWindow? mDebugWindow;

        public MainWindow()
        {
            SetServiceProvider();
            InitializeComponent();
            Hide();

            var fRes = Resources.Count;
            mRed = GetIcon("redIcon");
            mDefault = GetIcon("defaultIcon");

            //Check Credentials
            if(mServiceProvider == null) return;
            var fWindowsCredentials = mServiceProvider.GetService<IWindowsCredentialsService>();
            var fApiKey = fWindowsCredentials?.GetApiKey();
            if (fApiKey == string.Empty)
                Show();

            var fRoot = mServiceProvider.GetService<IRootService>();
            fRoot?.Update(myNotifyIcon, SetIcon);
            // Start Timer Loop
            mTimer = new Timer(60000);
            mTimer.Elapsed += (object? pSender, ElapsedEventArgs pE) =>
            {
                var fUpdateService = mServiceProvider.GetService<IUpdateService>();
                fUpdateService?.UpdateIfAvailable();
                fRoot?.Update(myNotifyIcon, SetIcon);
            };
            mTimer.AutoReset = true;
            mTimer.Start();
        }

        private void SetServiceProvider()
        {   
            var fServices = new ServiceCollection();
            fServices.AddSingleton<IRootService, RootService>();
            fServices.AddHttpClient<IConnectionService, ConnectionService>().ConfigureHttpClient(pX =>
            {
                pX.BaseAddress = new Uri("api-endpoint");
            });
            fServices.AddSingleton<IWindowsCredentialsService, WindowsCredentialService>();
            fServices.AddSingleton<IUpdateService, UpdateService>();
            mServiceProvider = fServices.BuildServiceProvider();
        }

        private void SetIcon(HttpStatusCode pStatusCode)
        {
            if (pStatusCode == HttpStatusCode.OK)
            {
                myNotifyIcon.Icon = mDefault;
                Dispatcher.Invoke(() => myNotifyIcon.ToolTipText = "(Connected)");
            }
            else
            {
                myNotifyIcon.Icon = mRed;
                Dispatcher.Invoke(() => myNotifyIcon.ToolTipText = "(Disconnected)");
            }
        }

        private void MyNotifyIcon_TrayMouseDoubleClick(object pSender, RoutedEventArgs pEvent)
        {
            if (mDebugWindow == null)
            {
                mDebugWindow = new DebugWindow();
                mDebugWindow.Closed += (pSender, pArgs) => mDebugWindow = null;
                mDebugWindow.Show();
            }
        }

        private Icon GetIcon(string pName)
        {
            var fIcon = Resources[pName] as BitmapImage;
            var fIleName = fIcon!.UriSource.ToString();
            return new Icon(fIleName, 29, 31);
        }

        private void SubmitCloudConnector_Click()
        {
            var fApiInputContent = ApiKeyInput.Text;
            ApiKeyInput.Text = "";
            var fWindowsCredentials = mServiceProvider?.GetService<IWindowsCredentialsService>();
            fWindowsCredentials?.SetApiKey(fApiInputContent);
            Hide();
        }

        private void SubmitButton_Click(object pSender, EventArgs pE) => SubmitCloudConnector_Click();

        private void SubmitEnter_Click(object pSender, KeyEventArgs pE)
        {
            if (pE.Key != Key.Enter) return;
            SubmitCloudConnector_Click();
        }

        private void ChangeCloudConnectorForm_Click(object pSender, EventArgs pE)
        {
            if (Visibility != Visibility.Visible) Show();
        }

        private void ExitCloudConnector_Click(object pSender, EventArgs pE) => Environment.Exit(0);

        protected override void OnClosing(CancelEventArgs pE)
        {
            base.OnClosing(pE);
            pE.Cancel = true;
            Hide();
        }
    }
}
