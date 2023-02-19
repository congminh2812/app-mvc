using AppMVC.DataAccess.Data;
using AppMVC.DataAccess.Repository.IRepository;

namespace AppMVC.DataAccess.Repository
{
    public class TypeRepository : Repository<Models.Type>, ITypeRepository
    {
        private readonly ApplicationDbContext _db;
        public TypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Models.Type obj)
        {
            _db.Types.Update(obj);
        }
    }
}
