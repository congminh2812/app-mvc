namespace AppMVC.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ITypeRepository Type { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        void Save();
    }
}
