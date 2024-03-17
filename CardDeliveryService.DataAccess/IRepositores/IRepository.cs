using CardDeliveryService.Domain.Commons;

namespace CardDeliveryService.DataAccess.IRepositores;

public interface IRepository<TEntity> where TEntity : Auditable
{
    ValueTask<TEntity> InsertAsync(TEntity entity);
    ValueTask<TEntity> UpdateAsync(TEntity entity);
    ValueTask<TEntity> DeleteAsync(TEntity entity);
    ValueTask<TEntity> DestroyAsync(TEntity entity);
    ValueTask<TEntity> SelectByIdAsync(long id);
    IQueryable<TEntity> SelectAllAsQueryable();
    IEnumerable<TEntity> SelectAllAsEnumerable();
    ValueTask SaveAsync();
}
