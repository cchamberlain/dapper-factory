using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Chambersoft.DapperFactory.Services
{
    /// <summary>
    /// Generic wrapper for DapperService that sets up support for deserializing to a default type when executing Query methods.
    /// </summary>
    /// <typeparam name="T">The type that Query related methods should deserialize to by default.</typeparam>
    public abstract class GenericDapperService<T> : DapperService where T : class
    {
        protected GenericDapperService(string connectionString)
            : base(connectionString)
        {
        }

        #region Sync Wrappers

        // Query Start
        protected IEnumerable<T> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        protected IEnumerable<T> Query(Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        protected IEnumerable<T> QueryFile(string path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return QueryFile<T>(path, param, transaction, buffered, commandTimeout, commandType);
        }

        protected IEnumerable<T> QueryFile(Enum path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return QueryFile<T>(path, param, transaction, buffered, commandTimeout, commandType);
        }

        protected IEnumerable<T> QuerySProc(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            return QuerySProc<T>(sql, param, transaction, buffered, commandTimeout);
        }

        protected IEnumerable<T> QuerySProc(Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            return QuerySProc<T>(sql, param, transaction, buffered, commandTimeout);
        }
        // Query End

        // CacheOrQuery Start
        protected IEnumerable<T> CacheOrQuery(string key, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQuery<T>(key, sql, param, transaction, buffered, commandTimeout, commandType, cacheTime);
        }

        protected IEnumerable<T> CacheOrQuery(string key, Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQuery<T>(key, sql, param, transaction, buffered, commandTimeout, commandType, cacheTime);
        }

        protected IEnumerable<T> CacheOrQueryFile(string key, string path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQueryFile<T>(key, path, param, transaction, buffered, commandTimeout, commandType, cacheTime);
        }

        protected IEnumerable<T> CacheOrQueryFile(string key, Enum path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQueryFile<T>(key, path, param, transaction, buffered, commandTimeout, commandType, cacheTime);
        }

        protected IEnumerable<T> CacheOrQuerySProc(string key, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQuerySProc<T>(key, sql, param, transaction, buffered, commandTimeout, cacheTime);
        }

        protected IEnumerable<T> CacheOrQuerySProc(string key, Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQuerySProc<T>(key, sql, param, transaction, buffered, commandTimeout, cacheTime);
        }
        // CacheOrQuery End

        #endregion

        #region Async wrappers

        // QueryAsync Start
        protected async Task<IEnumerable<T>> QueryAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> QueryAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> QueryFileAsync(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryFileAsync<T>(path, param, transaction, commandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> QueryFileAsync(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryFileAsync<T>(path, param, transaction, commandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> QuerySProcAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await QuerySProcAsync<T>(sql, param, transaction, commandTimeout);
        }

        protected async Task<IEnumerable<T>> QuerySProcAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await QuerySProcAsync<T>(sql, param, transaction, commandTimeout);
        }
        // QueryAsync End

        // CacheOrQueryAsync Start
        protected async Task<IEnumerable<T>> CacheOrQueryAsync(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryAsync<T>(key, sql, param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQueryAsync(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryAsync<T>(key, sql, param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQueryFileAsync(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryFileAsync<T>(key, path, param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQueryFileAsync(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryFileAsync<T>(key, path, param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQuerySProcAsync(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQuerySProcAsync<T>(key, sql, param, transaction, commandTimeout, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQuerySProcAsync(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            return await CacheOrQuerySProcAsync<T>(key, sql, param, transaction, commandTimeout, cacheTime);
        }

        // CacheOrQueryAsync End
        #endregion
    }
}
