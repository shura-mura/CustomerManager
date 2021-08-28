using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerManager.Data
{
    public abstract class EntityCollection<TEntity, TCollection> : IEntityCollection<TEntity, TCollection>
        where TEntity : class, IEntity
        where TCollection : class, IEntityCollection<TEntity, TCollection>
    {
        private readonly IQueryable<TEntity> _innerSet;

        protected EntityCollection(IQueryable<TEntity> innerSet, bool isReadonly) => _innerSet = isReadonly ? innerSet.AsNoTracking() : innerSet;

        public Type ElementType => _innerSet.ElementType;

        public Expression Expression => _innerSet.Expression;

        public IQueryProvider Provider => _innerSet.Provider;

        public IEnumerator<TEntity> GetEnumerator() => _innerSet.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_innerSet).GetEnumerator();

        public virtual TCollection Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) =>
            CreateCollection(EntityFrameworkQueryableExtensions.Include(this, navigationPropertyPath));

        public virtual TCollection Include<TProperty, TChildProperty>(
            Expression<Func<TEntity, IEnumerable<TProperty>>> navigationPropertyPath,
            Expression<Func<TProperty, TChildProperty>> childNavigationPropertyPath)
        {
            var query = EntityFrameworkQueryableExtensions.Include(this, navigationPropertyPath);
            return CreateCollection(query.ThenInclude(childNavigationPropertyPath));
        }

        protected abstract TCollection CreateCollection(IQueryable<TEntity> innerSet);
    }

    public class EntityCollection<TEntity> : EntityCollection<TEntity, IEntityCollection<TEntity>>, IEntityCollection<TEntity>
        where TEntity : class, IEntity
    {
        public EntityCollection(IQueryable<TEntity> innerSet, bool isReadonly)
            : base(innerSet, isReadonly)
        {
        }

        protected override IEntityCollection<TEntity> CreateCollection(IQueryable<TEntity> innerSet) => new EntityCollection<TEntity>(innerSet, false);
    }
}