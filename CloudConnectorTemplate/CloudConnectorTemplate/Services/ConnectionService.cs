using System.Net.Http;
using System.Threading.Tasks;
using CloudConnectorTemplate.Interface;

namespace CloudConnectorTemplate.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly HttpClient mHttpClient;

        public ConnectionService(HttpClient pHttpClient)
        {
            mHttpClient = pHttpClient;
        }

        public async Task<HttpResponseMessage> GetConnection(string pRelativePath)
        {
            var fRes = await mHttpClient.GetAsync(pRelativePath);
            return fRes;
        }
    }
}
