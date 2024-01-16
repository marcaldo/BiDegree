using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface IApiClientService
    {
        Task<IEnumerable<string>> GetFileList();
    }
}
