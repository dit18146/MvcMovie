using System.Data;
using System.Data.SqlClient;
using CSharpFunctionalExtensions;
using Dapper;

namespace MvcMovie.Services;

public class SqlHelper : ISqlHelper
{
    private readonly ILogger<SqlHelper> _logger;

    public SqlHelper(ILogger<SqlHelper> logger) => _logger = logger;

    /// <inheritdoc />
    public async Task<bool> IsSqlServerOkAsync(string connectionString)
    {
        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync().ConfigureAwait(false);
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
    }


    /// <inheritdoc />
    public async Task<Maybe<int>> ExecuteAsync(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        var result = await ExecuteViaDapperAsync(connectionString, sql,
            func => func.ExecuteAsync(sql, param, transaction, commandTimeout, commandType)).ConfigureAwait(false);

        return result;
    }

    /// <inheritdoc />
    public async Task<Maybe<object>> ExecuteScalarAsync(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        var result = await ExecuteViaDapperAsync<object>(connectionString, sql,
                func => func.ExecuteScalarAsync(sql, param, transaction, commandTimeout, commandType))
            .ConfigureAwait(false);

        return result;
    }

    /// <inheritdoc />
    public async Task<Maybe<T>> ExecuteScalarAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        var result = await ExecuteViaDapperAsync(connectionString, sql,
                func => func.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType))
            .ConfigureAwait(false);

        return result;
    }


    /// <inheritdoc />
    public async Task<Maybe<List<T>>> QueryAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        Maybe<List<T>> toReturn;

        var conn = new SqlConnection(connectionString);

        try
        {
            await conn.OpenAsync().ConfigureAwait(false);
            var result = await conn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType)
                .ConfigureAwait(false);

            toReturn = Maybe<List<T>>.From(result.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Data.SqlServer: {Sql}", sql);

            toReturn = Maybe<List<T>>.None;
        }
        finally
        {
            conn.Close();
        }

        return toReturn;
    }


    /// <inheritdoc />
    public async Task<Maybe<T>> QueryFirstAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        var result = await ExecuteViaDapperAsync(connectionString, sql,
                func => func.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType))
            .ConfigureAwait(false);

        return result;
    }


    /// <inheritdoc />
    public async Task<Maybe<T>> QueryFirstOrDefaultAsync<T>(string connectionString, string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        var result = await ExecuteViaDapperAsync(connectionString, sql,
                func => func.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType))
            .ConfigureAwait(false);

        return result;
    }


    /// <inheritdoc />
    public async Task<Maybe<T>> QuerySingleAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        var result = await ExecuteViaDapperAsync(connectionString, sql,
                func => func.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType))
            .ConfigureAwait(false);

        return result;
    }

    /// <inheritdoc />
    public async Task<Maybe<T>> QuerySingleOrDefaultAsync<T>(string connectionString, string sql,
        object? param = null,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var result = await ExecuteViaDapperAsync(connectionString, sql,
                func => func.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType))
            .ConfigureAwait(false);

        return result;
    }


    /// <inheritdoc />
    public async Task<(Maybe<List<T1>>, Maybe<List<T2>>)> QueryMultipleAsync<T1, T2>(string connectionString,
        string sql,
        object? param = null, IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        (Maybe<List<T1>>, Maybe<List<T2>>) toReturn;

        var conn = new SqlConnection(connectionString);

        try
        {
            await conn.OpenAsync().ConfigureAwait(false);
            var multi = await conn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType)
                .ConfigureAwait(false);

            var t1 = await multi.ReadAsync<T1>().ConfigureAwait(false);
            var t2 = await multi.ReadAsync<T2>().ConfigureAwait(false);

            var toReturnT1 = Maybe<List<T1>>.From(t1.ToList());
            var toReturnT2 = Maybe<List<T2>>.From(t2.ToList());

            toReturn = (toReturnT1, toReturnT2);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Data.SqlServer: {Sql}", sql);

            toReturn = (Maybe<List<T1>>.None, Maybe<List<T2>>.None);
        }
        finally
        {
            conn.Close();
        }

        return toReturn;
    }


    /// <inheritdoc />
    public async Task<(Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>)> QueryMultipleAsync<T1, T2, T3>(
        string connectionString,
        string sql,
        object? param = null, IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null)
    {
        (Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>) toReturn;

        var conn = new SqlConnection(connectionString);

        try
        {
            await conn.OpenAsync().ConfigureAwait(false);
            var multi = await conn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType)
                .ConfigureAwait(false);

            var t1 = await multi.ReadAsync<T1>().ConfigureAwait(false);
            var t2 = await multi.ReadAsync<T2>().ConfigureAwait(false);
            var t3 = await multi.ReadAsync<T3>().ConfigureAwait(false);

            var toReturnT1 = Maybe<List<T1>>.From(t1.ToList());
            var toReturnT2 = Maybe<List<T2>>.From(t2.ToList());
            var toReturnT3 = Maybe<List<T3>>.From(t3.ToList());

            toReturn = (toReturnT1, toReturnT2, toReturnT3);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Data.SqlServer: {Sql}", sql);

            toReturn = (Maybe<List<T1>>.None, Maybe<List<T2>>.None, Maybe<List<T3>>.None);
        }
        finally
        {
            conn.Close();
        }

        return toReturn;
    }


    /// <inheritdoc />
    public async Task<(Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>)>
        QueryMultipleAsync<T1, T2, T3, T4>(string connectionString, string sql, object? param = null,
            IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        (Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>) toReturn;

        var conn = new SqlConnection(connectionString);

        try
        {
            await conn.OpenAsync().ConfigureAwait(false);
            var multi = await conn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType)
                .ConfigureAwait(false);

            var t1 = await multi.ReadAsync<T1>().ConfigureAwait(false);
            var t2 = await multi.ReadAsync<T2>().ConfigureAwait(false);
            var t3 = await multi.ReadAsync<T3>().ConfigureAwait(false);
            var t4 = await multi.ReadAsync<T4>().ConfigureAwait(false);

            var toReturnT1 = Maybe<List<T1>>.From(t1.ToList());
            var toReturnT2 = Maybe<List<T2>>.From(t2.ToList());
            var toReturnT3 = Maybe<List<T3>>.From(t3.ToList());
            var toReturnT4 = Maybe<List<T4>>.From(t4.ToList());

            toReturn = (toReturnT1, toReturnT2, toReturnT3, toReturnT4);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Data.SqlServer: {Sql}", sql);

            toReturn = (Maybe<List<T1>>.None, Maybe<List<T2>>.None, Maybe<List<T3>>.None, Maybe<List<T4>>.None);
        }
        finally
        {
            conn.Close();
        }

        return toReturn;
    }

    /// <inheritdoc />
    public async Task<(Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>, Maybe<List<T5>>)>
        QueryMultipleAsync<T1, T2, T3, T4, T5>(string connectionString, string sql, object? param = null,
            IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        (Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>, Maybe<List<T5>>) toReturn;

        var conn = new SqlConnection(connectionString);

        try
        {
            await conn.OpenAsync().ConfigureAwait(false);
            var multi = await conn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType)
                .ConfigureAwait(false);

            var t1 = await multi.ReadAsync<T1>().ConfigureAwait(false);
            var t2 = await multi.ReadAsync<T2>().ConfigureAwait(false);
            var t3 = await multi.ReadAsync<T3>().ConfigureAwait(false);
            var t4 = await multi.ReadAsync<T4>().ConfigureAwait(false);
            var t5 = await multi.ReadAsync<T5>().ConfigureAwait(false);

            var toReturnT1 = Maybe<List<T1>>.From(t1.ToList());
            var toReturnT2 = Maybe<List<T2>>.From(t2.ToList());
            var toReturnT3 = Maybe<List<T3>>.From(t3.ToList());
            var toReturnT4 = Maybe<List<T4>>.From(t4.ToList());
            var toReturnT5 = Maybe<List<T5>>.From(t5.ToList());

            toReturn = (toReturnT1, toReturnT2, toReturnT3, toReturnT4, toReturnT5);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Data.SqlServer: {Sql}", sql);

            toReturn = (Maybe<List<T1>>.None, Maybe<List<T2>>.None, Maybe<List<T3>>.None, Maybe<List<T4>>.None,
                Maybe<List<T5>>.None);
        }
        finally
        {
            conn.Close();
        }

        return toReturn;
    }

    /// <inheritdoc />
    public async
        Task<(Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>, Maybe<List<T5>>, Maybe<List<T6>>)>
        QueryMultipleAsync<T1, T2, T3, T4, T5, T6>(string connectionString, string sql, object? param = null,
            IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        (Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>, Maybe<List<T5>>, Maybe<List<T6>>)
            toReturn;

        var conn = new SqlConnection(connectionString);

        try
        {
            await conn.OpenAsync().ConfigureAwait(false);
            var multi = await conn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType)
                .ConfigureAwait(false);

            var t1 = await multi.ReadAsync<T1>().ConfigureAwait(false);
            var t2 = await multi.ReadAsync<T2>().ConfigureAwait(false);
            var t3 = await multi.ReadAsync<T3>().ConfigureAwait(false);
            var t4 = await multi.ReadAsync<T4>().ConfigureAwait(false);
            var t5 = await multi.ReadAsync<T5>().ConfigureAwait(false);
            var t6 = await multi.ReadAsync<T6>().ConfigureAwait(false);

            var toReturnT1 = Maybe<List<T1>>.From(t1.ToList());
            var toReturnT2 = Maybe<List<T2>>.From(t2.ToList());
            var toReturnT3 = Maybe<List<T3>>.From(t3.ToList());
            var toReturnT4 = Maybe<List<T4>>.From(t4.ToList());
            var toReturnT5 = Maybe<List<T5>>.From(t5.ToList());
            var toReturnT6 = Maybe<List<T6>>.From(t6.ToList());

            toReturn = (toReturnT1, toReturnT2, toReturnT3, toReturnT4, toReturnT5, toReturnT6);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Data.SqlServer: {Sql}", sql);

            toReturn = (Maybe<List<T1>>.None, Maybe<List<T2>>.None, Maybe<List<T3>>.None, Maybe<List<T4>>.None,
                Maybe<List<T5>>.None, Maybe<List<T6>>.None);
        }
        finally
        {
            conn.Close();
        }

        return toReturn;
    }


    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connectionString"></param>
    /// <param name="sql"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    private async Task<Maybe<T>> ExecuteViaDapperAsync<T>(string connectionString, string sql,
        Func<IDbConnection, Task<T>> func)
    {
        if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
        if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException(nameof(sql));
        if (func == null) throw new ArgumentNullException(nameof(func));

        Maybe<T> toReturn;

        var conn = new SqlConnection(connectionString);

        try
        {
            await conn.OpenAsync().ConfigureAwait(false);

            var value = await func(conn).ConfigureAwait(false);

            toReturn = Maybe<T>.From(value);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Data.SqlServer: {Sql}", sql);

            toReturn = Maybe<T>.None;
        }
        finally
        {
            conn.Close();
        }

        return toReturn;
    }
}