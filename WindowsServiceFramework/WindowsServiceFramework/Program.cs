using Microsoft.Extensions.DependencyInjection;
using System;
using Topshelf;
using WindowsServiceFramework.Services;
using WindowsServiceFramework.Services.Interfaces;

namespace WindowsServiceFramework
{
    internal class Program
    {
        public static ServiceProvider mServiceProvider;

        static void Main(string[] pArgs)
        {
            var fServices = new ServiceCollection();
            fServices.AddHttpClient<IBlobService, BlobService>(p =>
            {
                p.BaseAddress = new Uri("storage-endpoint");
            });

            mServiceProvider = fServices.BuildServiceProvider();

              HostFactory.Run(p =>
            {
                p.StartAutomatically(); // Start the service automatically

                p.EnableServiceRecovery(pRc =>
                {
                    pRc.RestartService(TimeSpan.FromMilliseconds(100)); // restart the service after 1 minute
                });

                p.Service<CloudConnectorService>(pService =>
                {
                    var fDownloadService = mServiceProvider.GetService<IBlobService>();
                    pService.ConstructUsing(pS => new CloudConnectorService(fDownloadService));
                    pService.WhenStarted(pS => pS.Start());
                    pService.WhenStopped(pS => pS.Stop());
                });

                p.SetDescription("WindowsServiceFramework");
                p.SetDisplayName("WindowsServiceFramework");
                p.SetServiceName("WindowsServiceFramework");

            });

        }
    }
}
