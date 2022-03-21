using System;
using System.Net;
using Hardcodet.Wpf.TaskbarNotification;

namespace CloudConnectorTemplate_DNF.Interface
{
    public interface IRootService
    {
       void Update(TaskbarIcon pMyNotifyIcon, Action<HttpStatusCode> pToggleIcon);
    }
}
