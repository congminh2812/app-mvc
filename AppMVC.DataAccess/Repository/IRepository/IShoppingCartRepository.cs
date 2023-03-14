using AppMVC.Models;

namespace AppMVC.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        public int Increasement(ShoppingCart obj, int count);
        public int Decreasement(ShoppingCart obj, int count);
    }
}
