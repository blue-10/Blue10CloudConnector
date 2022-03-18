using System.Threading.Tasks;

namespace CloudConnectorTemplate.Interfaces
{
    internal interface IUpdateService
    {
        Task UpdateIfAvailable();
    }
}
