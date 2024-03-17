using CardDeliveryService.DataAccess.Contexts;
using CardDeliveryService.DataAccess.IRepositores;
using CardDeliveryService.Domain.Commons;
using Microsoft.EntityFrameworkCore;

namespace CardDeliveryService.DataAccess.Repositores;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    private readonly AppDbContext context;
    private readonly DbSet<TEntity> entities;

    public Repository(AppDbContext context, DbSet<TEntity> entities)
    {
        this.context = context;
        this.entities = context.Set<TEntity>();
    }

    public async ValueTask<TEntity> DeleteAsync(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entities.Entry(entity).State = EntityState.Modified;
        return await Task.FromResult(entity);
    }

    public async ValueTask<TEntity> DestroyAsync(TEntity entity)
    {
        return await Task.FromResult(entities.Remove(entity).Entity);
    }

    public async ValueTask<TEntity> InsertAsync(TEntity entity)
    {
        return (await entities.AddAsync(entity)).Entity;
    }

    public async ValueTask SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public IEnumerable<TEntity> SelectAllAsEnumerable()
    {
        return entities.Where(e => !e.IsDeleted).AsEnumerable();
    }

    public IQueryable<TEntity> SelectAllAsQueryable()
    {
        return entities.Where(e => !e.IsDeleted).AsQueryable();
    }

    public async ValueTask<TEntity> SelectByIdAsync(long id)
    {
        return await entities.Where(e => !e.IsDeleted).FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async ValueTask<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        entities.Entry(entity).State = EntityState.Modified;
        return await Task.FromResult(entity);
    }
}
