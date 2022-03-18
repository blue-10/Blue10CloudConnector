using System;
using System.Net;
using CloudConnectorTemplate.Interface;
using Hardcodet.Wpf.TaskbarNotification;

namespace CloudConnectorTemplate.Services
{
  public class RootService : IRootService
    {
        private readonly IConnectionService mConnection;
        private readonly IWindowsCredentialsService mWindowsCredentials;

        public RootService(IConnectionService pConnection, IWindowsCredentialsService pWindowsCredentials)
        {
            mConnection = pConnection;
            mWindowsCredentials = pWindowsCredentials;
        }

        public async void Update(TaskbarIcon pMyNotifyIcon, Action<HttpStatusCode> pToggleIcon)
        {
            var fRes = await mConnection.GetConnection("");
            var fStatus = fRes.StatusCode;
            pToggleIcon(fStatus);
        }
    }
}
