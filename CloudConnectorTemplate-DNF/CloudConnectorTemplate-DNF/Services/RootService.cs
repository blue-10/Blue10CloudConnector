using System;
using System.Net;
using CloudConnectorTemplate_DNF.Interface;
using Hardcodet.Wpf.TaskbarNotification;

namespace CloudConnectorTemplate_DNF.Services
{
    public class RootService : IRootService
    {
        private readonly IConnectionService mConnection;
        private readonly IAuthenticationService mWindowsCredentials;

        public RootService(IConnectionService pConnection, IAuthenticationService pWindowsCredentials)
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
