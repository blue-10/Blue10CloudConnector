using System.Net.Http;
using System.Threading.Tasks;

namespace WindowsServiceFramework.Services.Interfaces
{
    public interface IBlobService
    {
        Task<HttpResponseMessage> DownloadAsync(string pFileName, string url);
    }
}