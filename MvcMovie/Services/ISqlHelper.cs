using System.Data;
using CSharpFunctionalExtensions;

namespace MvcMovie.Services;

public interface ISqlHelper
{
    /// <summary>
    ///     Test that the sql server is connected
    /// </summary>
    /// <param name="connectionString">A connection string to database</param>
    /// <returns>true if the connection is opened</returns>
    Task<bool> IsSqlServerOkAsync(string connectionString);


    /// <summary>
    ///     Execute async parameterized SQL.
    /// </summary>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The number of rows affected.</returns>
    Task<Maybe<int>> ExecuteAsync(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null);


    /// <summary>
    ///     Execute parameterized SQL that selects a single value.
    /// </summary>
    /// ///
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell selected as <see cref="object" />.</returns>
    Task<Maybe<object>> ExecuteScalarAsync(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    ///     Execute parameterized SQL that selects a single value.
    /// </summary>
    /// ///
    /// <param name="connectionString">A connection string to database</param>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <typeparamref name="T" />.</returns>
    Task<Maybe<T>> ExecuteScalarAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null);


    /// <summary>
    ///     Execute a query asynchronously using .NET 4.5 Task.
    /// </summary>
    /// <typeparam name="T">The type of results to return.</typeparam>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <returns>
    ///     A sequence of data of <typeparamref name="T" />; if a basic type (int, string, etc) is queried then the data from
    ///     the first column in assumed, otherwise an instance is
    ///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
    /// </returns>
    Task<Maybe<List<T>>> QueryAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);


    /// <summary>
    ///     Execute a single-row query asynchronously using .NET 4.5 Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    Task<Maybe<T>> QueryFirstAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    ///     Execute a single-row query asynchronously using .NET 4.5 Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    Task<Maybe<T>> QueryFirstOrDefaultAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    ///     Execute a single-row query asynchronously using .NET 4.5 Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    Task<Maybe<T>> QuerySingleAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    ///     Execute a single-row query asynchronously using .NET 4.5 Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    Task<Maybe<T>> QuerySingleOrDefaultAsync<T>(string connectionString, string sql, object? param = null,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);


    /// <summary>
    ///     Execute a command that returns multiple result sets, and access each in turn.
    /// </summary>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    Task<(Maybe<List<T1>>, Maybe<List<T2>>)> QueryMultipleAsync<T1, T2>(string connectionString,
        string sql,
        object? param = null, IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    ///     Execute a command that returns multiple result sets asynchronously, and access each in turn.
    /// </summary>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    Task<(Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>)> QueryMultipleAsync<T1, T2, T3>(
        string connectionString, string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    ///     Execute a command that returns multiple result sets asynchronously, and access each in turn.
    /// </summary>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    Task<(Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>)> QueryMultipleAsync<T1, T2, T3, T4>(
        string connectionString, string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    ///     Execute a command that returns multiple result sets asynchronously, and access each in turn.
    /// </summary>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    Task<(Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>, Maybe<List<T5>>)>
        QueryMultipleAsync<T1, T2, T3, T4, T5>(string connectionString, string sql, object? param = null,
            IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    ///     Execute a command that returns multiple result sets asynchronously, and access each in turn.
    /// </summary>
    /// <param name="connectionString">A connection string to database</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    Task<(Maybe<List<T1>>, Maybe<List<T2>>, Maybe<List<T3>>, Maybe<List<T4>>, Maybe<List<T5>>, Maybe<List<T6>>)>
        QueryMultipleAsync<T1, T2, T3, T4, T5, T6>(string connectionString, string sql, object? param = null,
            IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
}