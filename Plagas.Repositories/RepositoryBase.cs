using Microsoft.EntityFrameworkCore;
using Plagas.Entities;
using System.Linq.Expressions;
using System.Linq;
namespace Plagas.Repositories;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
where TEntity: EntityBase
{
    protected readonly DbContext Context;

    protected RepositoryBase(DbContext context)
    {
        Context = context;
    }

    public virtual async Task<ICollection<TEntity>> ListAsync()
    {
        return await Context.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>()
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<TEntity>> ListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy)
    {
        return await Context.Set<TEntity>()
            .Where(predicate)
            .OrderBy(orderBy)
            .AsNoTracking()
            .ToListAsync();
    }








    public virtual async Task<TEntity?> FindByIdAsync(int id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task<int> AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity.Id;
    }

    public virtual async Task UpdateAsync()
    {
        //var affected = await Context.SaveChangesAsync();
        //if (affected == 0)
        //    throw new InvalidOperationException("No se actualizo nada");

        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var record = await FindByIdAsync(id);
        if (record is not null)
        {
            record.Status = false;
            await UpdateAsync();
        }
    }

    public async Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(
       Expression<Func<TEntity, bool>> predicate,
       Expression<Func<TEntity, TInfo>> selector,
       Expression<Func<TEntity, TKey>> orderby,
       int page, int rows,
       string? relationships)
    
    {

        var query = Context.Set<TEntity>()
            .Where(predicate)
            .IgnoreQueryFilters()
            .OrderBy(orderby)
            .Skip((page - 1) * rows)
            .Take(rows)
            .AsQueryable();

        

        if (!string.IsNullOrWhiteSpace(relationships))
        {
            foreach (var tabla in relationships.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(tabla);
            }
        }

        var collection = await query
            .Select(selector)
            .ToListAsync(); // Aqui recien se ejecuta el query

        var total = await Context.Set<TEntity>()
            .Where(predicate)
            .CountAsync();

        return (collection, total);
    }
















}