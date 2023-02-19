using AppMVC.Models;

namespace AppMVC.DataAccess.Repository.IRepository
{
    public interface ITypeRepository : IRepository<Models.Type>
    {
        void Update(Models.Type obj);
    }
}
