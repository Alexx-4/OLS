using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLatino.Admin.Application.Services
{
    public class WorkspaceFunctionService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<WorkspaceFunction> wfRepository;
        private IRepository<ServiceFunction> sfRepository;
        public WorkspaceFunctionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.wfRepository = unitOfWork.Set<WorkspaceFunction>();
            this.sfRepository = unitOfWork.Set<ServiceFunction>();
        }
        public void Add(int workspaceId, List<int> functionsIds)
        {
            foreach (var funcId in functionsIds)
            {
                wfRepository.Add(new WorkspaceFunction() { WorkSpaceId = workspaceId, FunctionId = funcId });
            }
            unitOfWork.SaveChanges();
        }
        public void Update(int workspaceId, IEnumerable<int> functionsIds)
        {
            var toRemove = wfRepository.GetAll(wf => wf.WorkSpaceId == workspaceId);
            foreach (var item in toRemove)
            {
                wfRepository.Remove(item);
            }
            foreach (var item in functionsIds)
            {
                wfRepository.Add(new WorkspaceFunction() { WorkSpaceId = workspaceId, FunctionId = item });
            }
            unitOfWork.SaveChanges();
        }
        public IEnumerable<ServiceFunction> GetServiceFuncions()
        {
            return sfRepository.GetAll();
        }
        public IEnumerable<ServiceFunction> GetServiceFunctionsForWorkspace(WorkSpace workSpace)
        {
            return workSpace.ServiceFunctions.Select(sf => sfRepository.Find(sf.FunctionId));
        }
        public int? GetServiceFunctionId(string name)
        {
            return sfRepository.GetAll(sf => sf.Name == name, true).Select(sf => sf.Id).FirstOrDefault();
        }
    }
}
