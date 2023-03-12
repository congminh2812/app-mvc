using AppMVC.DataAccess.Data;
using AppMVC.DataAccess.Repository.IRepository;

namespace AppMVC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public ITypeRepository Type { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Type = new TypeRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
