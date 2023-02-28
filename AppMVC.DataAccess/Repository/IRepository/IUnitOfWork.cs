namespace AppMVC.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ITypeRepository Type { get; }
        IProductRepository Product { get; }
        void Save();
    }
}
