using System.IO;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;
using WindowsServiceFramework.Services.Interfaces;

namespace WindowsServiceFramework.Services
{
    public class BlobService : IBlobService
    {
        public HttpClient mHttpClient;

        public BlobService(HttpClient pHttpClient)
        {
            mHttpClient = pHttpClient;
        }

        public async Task<HttpResponseMessage> DownloadAsync(string pFileName ,string pUrl)
        {
            var fRes = await mHttpClient.GetAsync(pUrl);

            using (var fStream = await mHttpClient.GetStreamAsync(pUrl))
            {
                using (var fFileStream = new FileStream($@"{Directory.GetCurrentDirectory()}\{pFileName}", FileMode.Create))
                {
                    await fStream.CopyToAsync(fFileStream);
                }
            }

            return fRes;
        }

    }
}
