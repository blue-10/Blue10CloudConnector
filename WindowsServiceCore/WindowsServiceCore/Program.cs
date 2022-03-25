using Topshelf;
using Microsoft.Extensions.DependencyInjection;

namespace WindowsServiceCore
{
    internal class Program
    {
        static void Main(string[] pArgs)
        {
            var fServices = new ServiceCollection();
            fServices.AddHttpClient<IBlobService, BlobService>(p =>
            {
                p.BaseAddress = new Uri("storage-endpoint");
            });

            var fServiceProvider = fServices.BuildServiceProvider();

            HostFactory.Run(p =>
            {
                p.StartAutomatically();

                p.EnableServiceRecovery(p => p.RestartService(TimeSpan.FromMilliseconds(100)));

                p.Service<CloudConnectorService>(pService =>
                {
                    var fBlobService = fServiceProvider.GetService<IBlobService>();
                    if (fBlobService == null) throw new Exception("Unable to get the blob service.");
                    pService.ConstructUsing(() => new CloudConnectorService(fBlobService));
                    pService.WhenStarted(p => p.Start());
                    pService.WhenStopped(p => p.Stop());
                });

                p.SetServiceName("CloudConnectorService");
            });
        }
    }
}



