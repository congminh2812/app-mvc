namespace AppMVC.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ITypeRepository Type { get; }
        void Save();
    }
}
