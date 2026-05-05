namespace Barbearia.Application.Interfaces.Repository;

public interface IRepositoryBase<T>
{
    Task<T> Add(T entity);
    Task<List<T>> GetAll();
    Task<T> Update(T entity);
    Task<T> GetById(int id);
    Task Delete(T entity);
}
