using CustomerManager.BusinessLogic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerManager.BusinessLogic.Data
{
    public interface IEntityCollection<TEntity, out TCollection> : IQueryable<TEntity>
        where TEntity : class, IEntity
        where TCollection : class, IEntityCollection<TEntity, TCollection>
    {
        TCollection Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath);

        TCollection Include<TProperty, TChildProperty>(
            Expression<Func<TEntity, IEnumerable<TProperty>>> navigationPropertyPath,
            Expression<Func<TProperty, TChildProperty>> childNavigationPropertyPath);
    }

    public interface IEntityCollection<TEntity> : IEntityCollection<TEntity, IEntityCollection<TEntity>>
        where TEntity : class, IEntity
    {
    }
}