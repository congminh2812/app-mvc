using AppMVC.DataAccess.Data;
using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;

namespace AppMVC.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var objFromDB = _db.Products.FirstOrDefault(s => s.Id == obj.Id);
            if (objFromDB is not null)
            {
                objFromDB.Title = obj.Title;
                objFromDB.Description = obj.Description;
                objFromDB.ISBN = obj.ISBN;
                objFromDB.Price = obj.Price;
                objFromDB.Price100 = obj.Price100;
                objFromDB.Price50 = obj.Price50;
                objFromDB.ListPrice = obj.ListPrice;
                objFromDB.Author = obj.Author;
                objFromDB.CategoryId = obj.CategoryId;
                objFromDB.TypeId = obj.TypeId;

                if (obj.ImageUrl is not null)
                    objFromDB.ImageUrl = obj.ImageUrl;

                _db.Products.Update(objFromDB);
            }
        }
    }
}
