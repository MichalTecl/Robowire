using System;
using System.Collections.Generic;
using System.Linq;

using Robowire.RobOrm.Core.EntityModel;
using Robowire.RobOrm.Core.Query.Abstraction;
using Robowire.RobOrm.Core.Query.Building;
using Robowire.RobOrm.Core.Query.Model;
using Robowire.RobOrm.Core.Query.Reader;

namespace Robowire.RobOrm.Core
{
    public abstract class DatabaseBase<TConnection> : IDatabase
    {
        private readonly IServiceLocator m_locator;
        private readonly IDataModelHelper m_dataModel;
        private readonly ITransactionManager<TConnection> m_transactionManager;

        protected DatabaseBase(IServiceLocator locator, IDataModelHelper dataModel, ITransactionManager<TConnection> transactionManager)
        {
            m_locator = locator;
            m_dataModel = dataModel;
            m_transactionManager = transactionManager;
        }

        public T New<T>() where T : class
        {
            return m_locator.Get<T>();
        }

        public IQueryBuilder<T> SelectFrom<T>() where T : class
        {
            var queryBuilder = new QueryBuilder<T>(m_dataModel, this);
            return queryBuilder;
        }

        public IEnumerable<T> Select<T>(IQueryModel<T> query) where T : class
        {
            List<T> result;
            using (var transaction = m_transactionManager.Open())
            {
                var hasBuilder = query as IHasBuilder<T>;
                if (hasBuilder == null)
                {
                    throw new NotSupportedException(
                              $"Unsupported query type {query.GetType()}. It must implement {typeof(IHasBuilder<T>)} interface");
                }

                using (var reader = ExecuteReader(query, hasBuilder.OwnerBuilder, transaction))
                {
                    result = ResultSetReader.Read<T>(reader, m_locator).ToList();
                }

                transaction.Commit();
            }

            return result;
        }

        public void Delete<T>(T entity) where T : class
        {
            var typedEntity = entity as IEntity;
            if (typedEntity == null)
            {
                throw new InvalidOperationException($"Cannot save {entity} - it should implement {typeof(IEntity)} interface");
            }

            using (var transaction = m_transactionManager.Open())
            {
                DeleteEntity(typedEntity, transaction);

                transaction.Commit();
            }
        }

        public void DeleteAll<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Save<T>(T entity) where T : class
        {
            var typedEntity = entity as IEntity;
            if (typedEntity == null)
            {
                throw new InvalidOperationException($"Cannot save {entity} - it should implement {typeof(IEntity)} interface");
            }
            
            var mode = typedEntity.GetSaveMethodType();

            using (var transaction = m_transactionManager.Open())
            {
                switch (mode)
                {
                    case SaveMethodType.Insert:
                        InsertEntity(typedEntity, transaction);
                        break;
                    case SaveMethodType.Update:
                        UpdateEntity(typedEntity, transaction);
                        break;
                    case SaveMethodType.Merge:
                        break;
                    default:
                        throw new NotSupportedException($"Unknown SaveMethodType {mode}");
                }

                transaction.Commit();
            }
        }


        public void SaveAll<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                Save(entity);
            }
        }

        public ITransaction OpenTransaction()
        {
            return m_transactionManager.Open();
        }

        public IEnumerable<TTarget> SelectSingleColumn<TSource, TTarget>(IQueryModel<TSource> query) where TSource : class
        {
            List<TTarget> result = new List<TTarget>();
            using (var transaction = m_transactionManager.Open())
            {
                var hasBuilder = query as IHasBuilder<TSource>;
                if (hasBuilder == null)
                {
                    throw new NotSupportedException(
                              $"Unsupported query type {query.GetType()}. It must implement {typeof(IHasBuilder<TSource>)} interface");
                }
                
                using (var reader = ExecuteReader<TSource>(query, hasBuilder.OwnerBuilder, transaction))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.Get<TTarget>(0));
                    }
                }

                transaction.Commit();
            }

            return result;
        }

        public abstract string GetQueryText<T>(IQueryModel<T> model, IQueryBuilder<T> builder) where T : class;

        protected abstract IDataReader ExecuteReader<T>(IQueryModel<T> model, IQueryBuilder<T> builder, ITransaction<TConnection> transaction) where T : class;

        protected abstract object InsertEntity(IEntity entity, ITransaction<TConnection> transaction);

        protected abstract void UpdateEntity(IEntity entity, ITransaction<TConnection> transaction);

        protected abstract object UpsertEntity(IEntity entity, ITransaction<TConnection> transaction);

        protected abstract void DeleteEntity(IEntity entity, ITransaction<TConnection> transaction);
    }
}
