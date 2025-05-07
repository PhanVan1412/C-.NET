using Microsoft.AspNetCore.Razor.TagHelpers;
using Npgsql;
using OrdersManagement.Common.Helper;
using OrdersManagement.Database;
using System.Reflection;

namespace OrdersManagement.DAO.Global
{
    public class BaseDAO
    {
        #region contructors
        IData objDataAccess;
        public BaseDAO()
        {
        }

        public BaseDAO(IData objDataAccess)
        {
            this.objDataAccess = objDataAccess;
        }
        #endregion

        /// <summary>
        /// Thực thi stored trả về Dictionnary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected async Task<List<Dictionary<string, object>>> ExecStoreToDictionnaryAsync(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            DateTime startTime = DateTime.Now;
            IData objData;
            if (objDataAccess == null)
                objData = Data.CreateData(connectionString ?? ConfigHelper.configModel.ConnectionStrings.ConnectionString);
            else
                objData = objDataAccess;

            string? exceptionMessage = null;

            try
            {
                if (objDataAccess == null)
                    await objData.BeginTransactionAsync();

                var data = await objData.ExecuteStoredDictionaryAsync(storedName, listParam, timeOut);

                if (objDataAccess == null)
                    await objData.CommitTransactionAsync();

                return data;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                if (objDataAccess == null)
                    await objData.RollbackTransactionAsync();

                if (exceptionMessage.Contains("permission denied") && objDataAccess == null)
                {
                    string connectionStringPri = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                    string connectionStringStandby = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;

                    if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionString)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                        return await this.ExecStoreToDictionnaryAsync(listParam, storedName, connectionString, timeOut);
                    }
                    else if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandby)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;
                        return await this.ExecStoreToDictionnaryAsync(listParam, storedName, connectionString, timeOut);
                    }
                }

                throw;
            }
            finally
            {
                if (objDataAccess == null)
                    await objData.DisposeDataSouceAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Thực thi stored trả về List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected async Task<List<T>> ExecStoreToObjectAsync<T>(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            DateTime startTime = DateTime.Now;

            IData objData;
            if (objDataAccess == null)
                objData = Data.CreateData(connectionString ?? ConfigHelper.configModel.ConnectionStrings.ConnectionString);
            else
                objData = objDataAccess;

            string? exceptionMessage = null;

            try
            {
                if (objDataAccess == null)
                    await objData.BeginTransactionAsync();

                var list = await objData.ExecuteStoredListAsync<T>(storedName, listParam, timeOut);

                if (objDataAccess == null)
                    await objData.CommitTransactionAsync();

                return list;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;

                if (objDataAccess == null)
                    await objData.RollbackTransactionAsync();

                if (exceptionMessage.Contains("permission denied") && objDataAccess == null)
                {
                    string connectionStringPri = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                    string connectionStringStandby = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;

                    if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionString)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                        return await this.ExecStoreToObjectAsync<T>(listParam, storedName, connectionString, timeOut);
                    }
                    else if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandby)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;
                        return await this.ExecStoreToObjectAsync<T>(listParam, storedName, connectionString, timeOut);
                    }
                }

                throw;
            }
            finally
            {
                if (objDataAccess == null)
                    await objData.DisposeDataSouceAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Thực thi stored trả về List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected async Task<List<T>> ExecStoreToObjectAsync<T>(List<NpgsqlParameter> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            DateTime startTime = DateTime.Now;

            IData objData;
            if (objDataAccess == null)
                objData = Data.CreateData(connectionString ?? ConfigHelper.configModel.ConnectionStrings.ConnectionString);
            else
                objData = objDataAccess;

            string? exceptionMessage = null;

            try
            {
                if (objDataAccess == null)
                    await objData.BeginTransactionAsync();

                var list = await objData.ExecuteStoredListAsync<T>(storedName, listParam, timeOut);

                if (objDataAccess == null)
                    await objData.CommitTransactionAsync();

                return list;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;

                if (objDataAccess == null)
                    await objData.RollbackTransactionAsync();

                if (exceptionMessage.Contains("permission denied") && objDataAccess == null)
                {
                    string connectionStringPri = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                    string connectionStringStandby = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;

                    if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionString)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                        return await this.ExecStoreToObjectAsync<T>(listParam, storedName, connectionString, timeOut);
                    }
                    else if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandby)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;
                        return await this.ExecStoreToObjectAsync<T>(listParam, storedName, connectionString, timeOut);
                    }
                }

                throw;
            }
            finally
            {
                if (objDataAccess == null)
                    await objData.DisposeDataSouceAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Thực thi stored trả về string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected async Task<string> ExecStoreToStringAsync(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            DateTime startTime = DateTime.Now;
            IData objData;
            if (objDataAccess == null)
                objData = Data.CreateData(connectionString ?? ConfigHelper.configModel.ConnectionStrings.ConnectionString);
            else
                objData = objDataAccess;

            string? exceptionMessage = null;

            try
            {
                if (objDataAccess == null)
                    await objData.BeginTransactionAsync();

                var output = await objData.ExecuteStoredStringAsync(storedName, listParam, timeOut);

                if (objDataAccess == null)
                    await objData.CommitTransactionAsync();

                return output;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;

                if (objDataAccess == null)
                    await objData.RollbackTransactionAsync();

                if (exceptionMessage.Contains("permission denied") && objDataAccess == null)
                {
                    string connectionStringPri = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                    string connectionStringStandby = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;

                    if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionString)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                        return await this.ExecStoreToStringAsync(listParam, storedName, connectionString, timeOut);
                    }
                    else if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandby)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;
                        return await this.ExecStoreToStringAsync(listParam, storedName, connectionString, timeOut);
                    }
                }

                throw;
            }
            finally
            {
                if (objDataAccess == null)
                    await objData.DisposeDataSouceAsync().ConfigureAwait(false);
            }
        }

        protected async Task<string> ExecStoreToStringAsync(List<NpgsqlParameter> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            DateTime startTime = DateTime.Now;
            IData objData;
            if (objDataAccess == null)
                objData = Data.CreateData(connectionString ?? ConfigHelper.configModel.ConnectionStrings.ConnectionString);
            else
                objData = objDataAccess;

            string? exceptionMessage = null;

            try
            {
                if (objDataAccess == null)
                    await objData.BeginTransactionAsync();

                var output = await objData.ExecuteStoredStringAsync(storedName, listParam, timeOut);

                if (objDataAccess == null)
                    await objData.CommitTransactionAsync();

                return output;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;

                if (objDataAccess == null)
                    await objData.RollbackTransactionAsync();

                if (exceptionMessage.Contains("permission denied") && objDataAccess == null)
                {
                    string connectionStringPri = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                    string connectionStringStandby = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;

                    if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionString)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                        return await this.ExecStoreToStringAsync(listParam, storedName, connectionString, timeOut);
                    }
                    else if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandby)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;
                        return await this.ExecStoreToStringAsync(listParam, storedName, connectionString, timeOut);
                    }
                }

                throw;
            }
            finally
            {
                if (objDataAccess == null)
                    await objData.DisposeDataSouceAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Thực thi stored trả về string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected async Task<object?> ExecStoreToScalarAsync(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            DateTime startTime = DateTime.Now;
            IData objData;
            if (objDataAccess == null)
                objData = Data.CreateData(connectionString ?? ConfigHelper.configModel.ConnectionStrings.ConnectionString);
            else
                objData = objDataAccess;

            string? exceptionMessage = null;

            try
            {
                if (objDataAccess == null)
                    await objData.BeginTransactionAsync();

                var output = await objData.ExecuteStoredScalarAsync(storedName, listParam, timeOut);

                if (objDataAccess == null)
                    await objData.CommitTransactionAsync();

                return output;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                if (objDataAccess == null)
                    await objData.RollbackTransactionAsync();

                if (exceptionMessage.Contains("permission denied") && objDataAccess == null)
                {
                    string connectionStringPri = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                    string connectionStringStandby = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;

                    if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionString)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                        return await this.ExecStoreToScalarAsync(listParam, storedName, connectionString, timeOut);
                    }
                    else if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandby)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;
                        return await this.ExecStoreToScalarAsync(listParam, storedName, connectionString, timeOut);
                    }
                }

                throw;
            }
            finally
            {
                if (objDataAccess == null)
                    await objData.DisposeDataSouceAsync().ConfigureAwait(false);

            }
        }

        /// <summary>
        /// Thực thi stored không trả về dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected async Task ExecStoreNoneQueryAsync(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            DateTime startTime = DateTime.Now;

            IData objData;
            if (objDataAccess == null)
                objData = Data.CreateData(connectionString ?? ConfigHelper.configModel.ConnectionStrings.ConnectionString);
            else
                objData = objDataAccess;

            string? exceptionMessage = null;

            try
            {
                if (objDataAccess == null)
                    await objData.BeginTransactionAsync();

                await objData.ExecuteStoredNonQueryAsync(storedName, listParam, timeOut);

                if (objDataAccess == null)
                    await objData.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;

                if (objDataAccess == null)
                    await objData.RollbackTransactionAsync();

                if (exceptionMessage.Contains("permission denied") && objDataAccess == null)
                {
                    string connectionStringPri = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                    string connectionStringStandby = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;

                    if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionString)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                        await this.ExecStoreNoneQueryAsync(listParam, storedName, connectionString, timeOut);
                        return;
                    }
                    else if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandby)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;
                        await this.ExecStoreNoneQueryAsync(listParam, storedName, connectionString, timeOut);
                        return;
                    }
                }

                throw;
            }
            finally
            {
                if (objDataAccess == null)
                    await objData.DisposeDataSouceAsync().ConfigureAwait(false);

            }
        }

        protected async Task ExecStoreNoneQueryAsync(List<NpgsqlParameter> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            DateTime startTime = DateTime.Now;

            IData objData;
            if (objDataAccess == null)
                objData = Data.CreateData(connectionString ?? ConfigHelper.configModel.ConnectionStrings.ConnectionString);
            else
                objData = objDataAccess;

            string? exceptionMessage = null;

            try
            {
                if (objDataAccess == null)
                    await objData.BeginTransactionAsync();

                await objData.ExecuteStoredNonQueryAsync(storedName, listParam, timeOut);

                if (objDataAccess == null)
                    await objData.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;

                if (objDataAccess == null)
                    await objData.RollbackTransactionAsync();

                if (exceptionMessage.Contains("permission denied") && objDataAccess == null)
                {
                    string connectionStringPri = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                    string connectionStringStandby = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;

                    if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionString)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringOld;
                        await this.ExecStoreNoneQueryAsync(listParam, storedName, connectionString, timeOut);
                        return;
                    }
                    else if (connectionString == ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandby)
                    {
                        connectionString = ConfigHelper.configModel.ConnectionStrings.ConnectionStringStandbyOld;
                        await this.ExecStoreNoneQueryAsync(listParam, storedName, connectionString, timeOut);
                        return;
                    }
                }

                throw;
            }
            finally
            {
                if (objDataAccess == null)
                    await objData.DisposeDataSouceAsync().ConfigureAwait(false);
            }
        }


        protected List<T> ExecStoreToObject<T>(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            var task = this.ExecStoreToObjectAsync<T>(listParam, storedName, connectionString, timeOut);
            return task.Result;
        }

        protected List<Dictionary<string, object>> ExecStoreToDictionnary(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            var task = this.ExecStoreToDictionnaryAsync(listParam, storedName, connectionString, timeOut);
            return task.Result;
        }
        //protected DataTable ExecStoreToDataTable(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        //{
        //    var task = this.ExecStoreToDataTableAsync(listParam, storedName, connectionString, timeOut);
        //    return task.Result;
        //}

        /// <summary>
        /// Thực thi stored không trả về dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected void ExecStoreNoneQuery(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            var task = this.ExecStoreNoneQueryAsync(listParam, storedName, connectionString, timeOut);
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Thực thi stored không trả về dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected void ExecStoreNoneQuery(List<NpgsqlParameter> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            var task = this.ExecStoreNoneQueryAsync(listParam, storedName, connectionString, timeOut);
            task.GetAwaiter().GetResult();
        }


        /// <summary>
        /// Thực thi stored trả về string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedName">Tên stored</param>
        /// <param name="listParam">params</param>
        /// <param name="connectionString">connectionString</param>
        /// <param name="timeOut">Thời gian timeout (s)</param>
        /// <returns></returns>
        protected string ExecStoreToString(List<object> listParam, string storedName, string? connectionString = null, int timeOut = 90)
        {
            var task = this.ExecStoreToStringAsync(listParam, storedName, connectionString, timeOut);
            return task.Result;
        }

        public void WriteDynamicValue(NpgsqlBinaryImporter writer, dynamic value)
        {
            if (value == null)
                writer.WriteNull();
            else if (value is string)
                writer.Write(value, NpgsqlTypes.NpgsqlDbType.Text);
            else if (value is int)
                writer.Write(value, NpgsqlTypes.NpgsqlDbType.Integer);
            else if (value is Int16)
                writer.Write(value, NpgsqlTypes.NpgsqlDbType.Smallint);
            else if (value is Int64)
                writer.Write(value, NpgsqlTypes.NpgsqlDbType.Bigint);
            else if (value is DateTime)
                writer.Write(value, NpgsqlTypes.NpgsqlDbType.Timestamp);
            else if (value is bool)
                writer.Write(value, NpgsqlTypes.NpgsqlDbType.Boolean);
            else if (value is double)
                writer.Write(value, NpgsqlTypes.NpgsqlDbType.Double);
            else
                throw new InvalidOperationException($"Unsupported data type: {value.GetType()}");
        }
    }
}
