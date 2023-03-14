using AppMVC.DataAccess.Data;
using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;

namespace AppMVC.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int Decreasement(ShoppingCart obj, int count)
        {
            obj.Count -= count;
            return obj.Count;
        }

        public int Increasement(ShoppingCart obj, int count)
        {
            obj.Count += count;
            return obj.Count;
        }
    }
}
