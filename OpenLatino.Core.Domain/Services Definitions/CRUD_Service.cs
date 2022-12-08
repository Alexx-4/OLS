using System.Collections.Generic;

namespace OpenLatino.Core.Domain.Interfaces
{
    public abstract class CRUD_Service<TClass> where TClass: class
    {

        protected IRepository<TClass> repository;
        protected IUnitOfWork unitOfWork;
        public CRUD_Service(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.repository = unitOfWork.Set<TClass>();
        }

        public virtual void Create(TClass obj)
        {
            repository.Add(obj);
            unitOfWork.SaveChanges();
        }

        public virtual void Remove(TClass obj)
        {
            repository.Remove(obj);
            unitOfWork.SaveChanges();
        }

        public virtual void Update(TClass obj)
        {
            repository.Modify(obj);
            unitOfWork.SaveChanges();
        }

        public virtual IEnumerable<TClass> List()
        {
            return repository.GetAll(obj => true, true);
        }

        public virtual object GetById(int? id)
        {
            if (id == null)
                return null;
            return repository.Find(id);
        }
    }
}
