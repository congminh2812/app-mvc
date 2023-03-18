using AppMVC.DataAccess.Data;
using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;

namespace AppMVC.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDB = _db.OrderHeaders.FirstOrDefault(s => s.Id == id);

            if (orderFromDB is not null)
            {
                orderFromDB.OrderStatus = orderStatus;

                if (paymentStatus is not null)
                    orderFromDB.PaymentStatus = paymentStatus;
            }
        }
    }
}
