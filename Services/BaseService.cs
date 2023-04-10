using Microsoft.EntityFrameworkCore;
using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

public abstract class BaseService<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly TestDbContext _ctx;
    protected DbSet<TEntity> _dbSet;
    protected List<TEntity> items = new();
    protected int pageSize = 10;
    protected int totalCount;
    protected bool hasNext;
    

    public BaseService(TestDbContext ctx)
    {
        _ctx = ctx;
        _dbSet = _ctx.Set<TEntity>();
    }

    public virtual ItemList<TEntity> ListItems(int page)
    {
        if (page < 1) page = 1;
        int totalSize = _dbSet.Count();
        int skip = (page - 1) * pageSize;
        hasNext = (skip + pageSize) < totalSize;
        if (hasNext)
        {
            totalCount = pageSize;
        } else
        {
            totalCount = totalSize - skip;
        }
        items = _dbSet.OrderBy(p => p.Id).Skip(skip).Take(pageSize).ToList();

        return new ItemList<TEntity>() { HasNext = hasNext, TotalCount = totalCount, Items = items };
    }

}

