using System.Net.Http;
using System.Threading.Tasks;

namespace CloudConnectorTemplate.Interface
{
    public interface IConnectionService
    {
        Task<HttpResponseMessage> GetConnection(string pRelativePath);
    }
}
