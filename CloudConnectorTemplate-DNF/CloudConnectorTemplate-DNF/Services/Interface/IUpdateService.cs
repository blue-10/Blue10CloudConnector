using System.Threading.Tasks;

namespace CloudConnectorTemplate_DNF.Interfaces
{
    internal interface IUpdateService
    {
        Task UpdateIfAvailable();
    }
}
