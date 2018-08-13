using System;
using System.Collections.Generic;
using System.Data.Common;

using Robowire.RobOrm.Core.Query.Abstraction;

namespace Robowire.RobOrm.Core
{
    public interface IDatabase
    {
        T New<T>() where T : class;

        IQueryBuilder<T> SelectFrom<T>() where T : class;

        IEnumerable<T> Select<T>(IQueryModel<T> query) where T : class;

        void Delete<T>(T entity) where T : class;

        void DeleteAll<T>(IEnumerable<T> entities) where T : class;

        void Save<T>(T entity) where T : class;

        void SaveAll<T>(IEnumerable<T> entities) where T : class;
        
        ITransaction OpenTransaction();

        IEnumerable<TTarget> SelectSingleColumn<TSource, TTarget>(IQueryModel<TSource> model) where TSource : class;

        string GetQueryText<T>(IQueryModel<T> model, IQueryBuilder<T> builder) where T : class;

        object ExecuteScalar(string query, Action<DbParameterCollection> setParameters);
    }
}
