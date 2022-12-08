using OpenLatino.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLatino.Admin.Application.Services
{
    public class InfoService
    {
        private WorkspaceService _workspaceService;
        public InfoService(IUnitOfWork unitOfWork)
        {
            _workspaceService = new WorkspaceService(unitOfWork);
        }


        public async Task<IEnumerable<object>> GetAbleFunctions(int workspaceId)
        {
            var workspace = await Task.Run(()=> _workspaceService.GetWorkspaceFullInfo(workspaceId));
            return workspace.ServiceFunctions.Select(sf => new { Protocol = sf.Service.Name, Name = sf.Name });
        }
    }
}
