using System.Net.Http;
using System.Threading.Tasks;

namespace CloudConnectorTemplate_DNF.Interface
{
    public interface IConnectionService
    {
        Task<HttpResponseMessage> GetConnection(string pRelativePath);
    }
}
