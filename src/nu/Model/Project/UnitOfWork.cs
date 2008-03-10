using System;
using System.Collections.Generic;

namespace nu.Model.Project
{
    public static class UnitOfWork
    {
        private static readonly IDictionary<Type, Object> registry = new Dictionary<Type, Object>();

        public static void RegisterItem<TEntity>(TEntity entity)
        {
            if (registry.ContainsKey(typeof(TEntity)))
                throw new ArgumentException("Type:{0} has already been registered.", typeof(TEntity).ToString());
            registry.Add(typeof(TEntity), entity);
        }

        public static TEntity GetItem<TEntity>()
        {
            if (!registry.ContainsKey(typeof(TEntity)))
                throw new ArgumentException("Type {0} has not been registered.", typeof(TEntity).ToString());
            return (TEntity)registry[typeof(TEntity)];
        }

        public static void Reset()
        {
            registry.Clear();
        }
    }
}