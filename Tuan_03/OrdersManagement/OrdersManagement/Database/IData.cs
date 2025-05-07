using Npgsql;
using System.Data;

namespace OrdersManagement.Database
{
    public interface IData
    {
        NpgsqlDataSource dataSource { get; set; }
        /// <summary>
        /// nội dung RAISE notice từ PostgreSQL
        /// </summary>
        string notice { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task DisposeConnectionAsync();
        Task DisposeDataSouceAsync();
        /// <summary>
        /// Thực thi danh sách stored trong 1 lần gọi
        /// </summary>
        /// <param name="listCommand">Danh sách stored + params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        Task ExecuteListStoredNoneQueryAsync(List<Data.BatchCommandStoredModel> listCommand, int timeOut = 90);
        /// <summary>
        /// Thực thi stored trả về List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        Task<List<T>> ExecuteStoredListAsync<T>(string storedName, List<object> listParam, int timeOut = 90);
        /// <summary>
        /// Thực thi stored trả về List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        Task<List<T>> ExecuteStoredListAsync<T>(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90);
        /// <summary>
        /// Thực thi stored trả về Dictionnary (uppercase)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <param name="isUpperCase">UpperCase Columns</param>
        /// <returns></returns>
        Task<List<Dictionary<string, object>>> ExecuteStoredDictionaryAsync(string storedName, List<object> listParam, int timeOut = 90, bool isUpperCase = true);

        /// <summary>
        /// Thực thi stored trả về Dictionnary (uppercase)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <param name="isUpperCase">UpperCase Columns</param>
        /// <returns></returns>
        Task<DataTable> ExecuteStoredDataTableAsync(string storedName, List<object> listParam, int timeOut = 90, bool isUpperCase = true);

        /// <summary>
        /// Thực thi stored không nhận dữ liệu trả về
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        Task ExecuteStoredNonQueryAsync(string storedName, List<object> listParam, int timeOut = 90);

        /// <summary>
        /// Thực thi stored không nhận dữ liệu trả về
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        Task ExecuteStoredNonQueryAsync(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90);

        /// <summary>
        /// Thực thi stored trả về dữ liệu ô đầu tiên (object?)
        /// </summary>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        Task<object?> ExecuteStoredScalarAsync(string storedName, List<object> listParam, int timeOut = 90);
        /// <summary>
        /// Thực thi stored trả về dữ liệu ô đầu tiên (string)
        /// </summary>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>

        Task<string> ExecuteStoredStringAsync(string storedName, List<object> listParam, int timeOut = 90);

        /// <summary>
        /// Thực thi stored trả về dữ liệu ô đầu tiên (string)
        /// </summary>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        Task<string> ExecuteStoredStringAsync(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90);
    }
}
