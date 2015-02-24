using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Chambersoft.DapperFactory.Dependencies;
using Chambersoft.DapperFactory.Extensions;
using Chambersoft.DapperFactory.Providers;
using Dapper;

namespace Chambersoft.DapperFactory.Services
{
    /// <summary>
    /// Wrapper for dapper service calls.
    /// </summary>
    /// <remarks>
    /// Supports sync/async versions of Query, CacheOrQuery, Execute, ExecuteScalar, CacheOrExecuteScalar, QueryMultiple, CacheOrQueryMultiple and includes built-in support for reading SQL from enums and text files for each of the methods.
    /// </remarks>
    public abstract class DapperService
    {
        protected const int DefaultCacheTime = 360;

        protected ICacheProvider CacheProvider { get; private set; }
        protected string ConnectionString { get; private set; }

        protected DapperService(string connectionString)
        {
            CacheProvider = Injection.Get<ICacheProvider>();
            ConnectionString = connectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        #region Sync Wrappers

        // Query Start
        protected IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        protected IEnumerable<T> Query<T>(Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Query<T>(sql.GetDescription(), param, transaction, buffered, commandTimeout, commandType);
        }

        protected IEnumerable<T> QueryFile<T>(string path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Query<T>(path.ReadFileContents(), param, transaction, buffered, commandTimeout, commandType);
        }

        protected IEnumerable<T> QueryFile<T>(Enum path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return QueryFile<T>(path.GetDescription(), param, transaction, buffered, commandTimeout, commandType);
        }

        protected IEnumerable<T> QuerySProc<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            return Query<T>(sql, param, transaction, buffered, commandTimeout, CommandType.StoredProcedure);
        }

        protected IEnumerable<T> QuerySProc<T>(Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            return QuerySProc<T>(sql.GetDescription(), param, transaction, buffered, commandTimeout);
        }
        // Query End

        // CacheOrQuery Start
        protected IEnumerable<T> CacheOrQuery<T>(string key, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheProvider.GetWithRefresh(key, () => Query<T>(sql, param, transaction, buffered, commandTimeout, commandType), cacheTime);
        }

        protected IEnumerable<T> CacheOrQuery<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrQuery<T>(key, sql.GetDescription(), param, transaction, buffered, commandTimeout, commandType, cacheTime);
        }

        protected IEnumerable<T> CacheOrQueryFile<T>(string key, string path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrQuery<T>(key, path.ReadFileContents(), param, transaction, buffered, commandTimeout, commandType, cacheTime);
        }

        protected IEnumerable<T> CacheOrQueryFile<T>(string key, Enum path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrQueryFile<T>(key, path.GetDescription(), param, transaction, buffered, commandTimeout, commandType, cacheTime);
        }

        protected IEnumerable<T> CacheOrQuerySProc<T>(string key, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrQuery<T>(key, sql, param, transaction, buffered, commandTimeout, CommandType.StoredProcedure, cacheTime);
        }

        protected IEnumerable<T> CacheOrQuerySProc<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrQuerySProc<T>(key, sql.GetDescription(), param, transaction, buffered, commandTimeout, cacheTime);
        }
        // CacheOrQuery End

        // Execute Start
        protected int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Execute(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected int Execute(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Execute(sql.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected int ExecuteFile(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Execute(path.ReadFileContents(), param, transaction, commandTimeout, commandType);
        }

        protected int ExecuteFile(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ExecuteFile(path.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected int ExecuteSProc(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Execute(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        protected int ExecuteSProc(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return ExecuteSProc(sql.GetDescription(), param, transaction, commandTimeout);
        }
        // Execute End

        // ExecuteScalar Start
        protected T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = GetConnection())
            {
                return connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected T ExecuteScalar<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ExecuteScalar<T>(sql.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected T ExecuteScalarFile<T>(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ExecuteScalar<T>(path.ReadFileContents(), param, transaction, commandTimeout, commandType);
        }

        protected T ExecuteScalarFile<T>(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ExecuteScalarFile<T>(path.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected T ExecuteScalarSProc<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return ExecuteScalar<T>(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        protected T ExecuteScalarSProc<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return ExecuteScalarSProc<T>(sql.GetDescription(), param, transaction, commandTimeout);
        }
        // ExecuteScalar End

        // CacheOrExecuteScalar Start
        protected T CacheOrExecuteScalar<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheProvider.GetWithRefresh(key, () => ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType), cacheTime);
        }

        protected T CacheOrExecuteScalar<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrExecuteScalar<T>(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected T CacheOrExecuteScalarFile<T>(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrExecuteScalar<T>(key, path.ReadFileContents(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected T CacheOrExecuteScalarFile<T>(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrExecuteScalarFile<T>(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected T CacheOrExecuteScalarSProc<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrExecuteScalar<T>(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);
        }

        protected T CacheOrExecuteScalarSProc<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return CacheOrExecuteScalarSProc<T>(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);
        }
        // CacheOrExecuteScalar End

        // QueryMultiple Start
        protected SqlMapper.GridReader QueryMultiple(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected SqlMapper.GridReader QueryMultiple(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return QueryMultiple(sql.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected SqlMapper.GridReader QueryMultipleFile(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return QueryMultiple(path.ReadFileContents(), param, transaction, commandTimeout, commandType);
        }

        protected SqlMapper.GridReader QueryMultipleFile(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return QueryMultipleFile(path.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected SqlMapper.GridReader QueryMultipleSProc(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return QueryMultiple(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        protected SqlMapper.GridReader QueryMultipleSProc(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return QueryMultipleSProc(sql.GetDescription(), param, transaction, commandTimeout);
        }
        // QueryMultiple End

        // CacheOrQueryMultiple Start
        protected SqlMapper.GridReader CacheOrQueryMultiple(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return CacheProvider.GetWithRefresh(key, () => QueryMultiple(sql, param, transaction, commandTimeout, commandType), cacheTime);
        }

        protected SqlMapper.GridReader CacheOrQueryMultiple(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQueryMultiple(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected SqlMapper.GridReader CacheOrQueryMultipleFile(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQueryMultiple(key, path.ReadFileContents(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected SqlMapper.GridReader CacheOrQueryMultipleFile(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQueryMultipleFile(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected SqlMapper.GridReader CacheOrQueryMultipleSProc(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQueryMultiple(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);
        }

        protected SqlMapper.GridReader CacheOrQueryMultipleSProc(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
        {
            return CacheOrQueryMultipleSProc(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);
        }
        // CacheOrQueryMultiple End

        #endregion

        #region Async wrappers

        // QueryAsync Start
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = GetConnection())
            {
                return await connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryAsync<T>(sql.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> QueryFileAsync<T>(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryAsync<T>(await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> QueryFileAsync<T>(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryFileAsync<T>(path.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> QuerySProcAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await QueryAsync<T>(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        protected async Task<IEnumerable<T>> QuerySProcAsync<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await QuerySProcAsync<T>(sql.GetDescription(), param, transaction, commandTimeout);
        }
        // QueryAsync End

        // CacheOrQueryAsync Start
        protected async Task<IEnumerable<T>> CacheOrQueryAsync<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheProvider.GetWithRefreshAsync(key, async () => await QueryAsync<T>(sql, param, transaction, commandTimeout, commandType), cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQueryAsync<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrQueryAsync<T>(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQueryFileAsync<T>(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrQueryAsync<T>(key, await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQueryFileAsync<T>(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrQueryFileAsync<T>(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQuerySProcAsync<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrQueryAsync<T>(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);
        }

        protected async Task<IEnumerable<T>> CacheOrQuerySProcAsync<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrQuerySProcAsync<T>(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);
        }
        // CacheOrQueryAsync End

        // ExecuteAsync Start
        protected async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = GetConnection())
            {
                return await connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<int> ExecuteAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await ExecuteAsync(sql.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<int> ExecuteFileAsync(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await ExecuteAsync(await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<int> ExecuteFileAsync(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await ExecuteFileAsync(path.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<int> ExecuteSProcAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await ExecuteAsync(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        protected async Task<int> ExecuteSProcAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await ExecuteSProcAsync(sql.GetDescription(), param, transaction, commandTimeout);
        }
        // ExecuteAsync End

        // ExecuteScalarAsync Start
        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = GetConnection())
            {
                return await connection.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<T> ExecuteScalarAsync<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await ExecuteScalarAsync<T>(sql.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<T> ExecuteScalarFileAsync<T>(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await ExecuteScalarAsync<T>(await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<T> ExecuteScalarFileAsync<T>(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await ExecuteScalarFileAsync<T>(path.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<T> ExecuteScalarSProcAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        protected async Task<T> ExecuteScalarSProcAsync<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await ExecuteScalarSProcAsync<T>(sql.GetDescription(), param, transaction, commandTimeout);
        }
        // ExecuteScalarAsync End

        // CacheOrExecuteScalarAsync Start
        protected async Task<T> CacheOrExecuteScalarAsync<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheProvider.GetWithRefreshAsync(key, async () => await ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType), cacheTime);
        }

        protected async Task<T> CacheOrExecuteScalarAsync<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrExecuteScalarAsync<T>(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<T> CacheOrExecuteScalarFileAsync<T>(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrExecuteScalarAsync<T>(key, await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<T> CacheOrExecuteScalarFileAsync<T>(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrExecuteScalarFileAsync<T>(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<T> CacheOrExecuteScalarSProcAsync<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrExecuteScalarAsync<T>(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);
        }

        protected async Task<T> CacheOrExecuteScalarSProcAsync<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
        {
            return await CacheOrExecuteScalarSProcAsync<T>(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);
        }
        // CacheOrExecuteScalarAsync End

        // QueryMultipleAsync Start
        protected async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = GetConnection())
            {
                return await connection.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<SqlMapper.GridReader> QueryMultipleAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryMultipleAsync(sql.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<SqlMapper.GridReader> QueryMultipleFileAsync(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryMultipleAsync(await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<SqlMapper.GridReader> QueryMultipleFileAsync(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await QueryMultipleFileAsync(path.GetDescription(), param, transaction, commandTimeout, commandType);
        }

        protected async Task<SqlMapper.GridReader> QueryMultipleSProcAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await QueryMultipleAsync(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        protected async Task<SqlMapper.GridReader> QueryMultipleSProcAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await QueryMultipleSProcAsync(sql.GetDescription(), param, transaction, commandTimeout);
        }
        // QueryMultipleAsync End

        // CacheOrQueryMultipleAsync Start
        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleAsync(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheProvider.GetWithRefreshAsync(key, async () => await QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType), cacheTime);
        }

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleAsync(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryMultipleAsync(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleFileAsync(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryMultipleAsync(key, await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleFileAsync(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryMultipleFileAsync(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);
        }

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleSProcAsync(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryMultipleAsync(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);
        }

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleSProcAsync(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
        {
            return await CacheOrQueryMultipleSProcAsync(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);
        }
        // CacheOrQueryMultipleAsync End
        #endregion
    }
}
