using Npgsql;
using NpgsqlTypes;
using OrdersManagement.Common.Helper;
using System.Data;

namespace OrdersManagement.Database
{
    public class PosgreSQLData :  Data, IData
    {
        public NpgsqlDataSource dataSource { get; set; }
        public NpgsqlConnection? connection;
        private NpgsqlTransaction? transaction;

        /// <summary>
        /// nội dung RAISE notice từ PostgreSQL
        /// </summary>
        public string notice { get; internal set; }


        public PosgreSQLData(string _connectionString)
        {
            foreach (var item in PostgreSQLService.Instance.dictDataSource)
            {
                if (_connectionString == item.Value.ConnectionString)
                {
                    dataSource = item.Value.dataSource;
                    //dataSource = NpgsqlDataSource.Create(_connectionString);
                    break;
                }
            }

            notice = "";
        }

        public async Task DisposeDataSouceAsync()
        {
            await DisposeConnectionAsync();
            //await dataSource.DisposeAsync();
        }

        public async Task DisposeConnectionAsync()
        {
            if (connection != null)
            {
                await connection.DisposeAsync();
                await connection.CloseAsync();
                connection = null;
            }
        }

        private async Task OpenConnectionAsync()
        {
            if (connection == null)
            {
                connection = await dataSource.OpenConnectionAsync();
                connection.Notice += Connection_Notice; // Subscribe to the Notice event
            }
        }

        private void Connection_Notice(object? sender, NpgsqlNoticeEventArgs e)
        {
            notice += e.Notice.MessageText + "\n";
        }

        public async Task BeginTransactionAsync()
        {
            await OpenConnectionAsync();
            if (transaction == null && connection != null)
            {
                transaction = await connection.BeginTransactionAsync();
                notice = "";
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }


        private async Task<NpgsqlCommand> GenCommandAsync(string storedName, List<object> listParam, int timeOut = 90)
        {
            await BeginTransactionAsync();

            storedName = $"{storedName}({listParam.Select((x, index) => $"${index + 1}").Join()})";

            var command = new NpgsqlCommand($"SELECT {storedName}", connection, transaction);

            SetParameters(command, listParam);

            command.CommandTimeout = timeOut;
            return command;
        }

        /// <summary>
        /// Set các parameter cho command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="listParam"></param>
        private void SetParameters(NpgsqlCommand command, List<object> listParam)
        {
            foreach (var param in listParam)
            {
                NpgsqlParameter npgsqlParameter = new NpgsqlParameter();
                SetTypeParameter(npgsqlParameter, param);
                npgsqlParameter.Value = param ?? DBNull.Value;
                command.Parameters.Add(npgsqlParameter);
            }
        }

        /// <summary>
        /// Xử lý type dữ liệu của parameter đối với các trường hợp đặc biệt 
        /// </summary>
        /// <param name="npgsqlParameter"></param>
        /// <param name="param"></param>
        private void SetTypeParameter(NpgsqlParameter npgsqlParameter, object param)
        {
            if (param is TimeSpan)
            {
                npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Time;
            }
            else if (param is double || param is decimal)
            {
                npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Numeric;
            }
        }

        private async Task<NpgsqlCommand> GenCommandAsync(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90)
        {
            await BeginTransactionAsync();

            storedName = $"{storedName}({listParam.Select((x, index) => $"${index + 1}").Join()})";

            var command = new NpgsqlCommand($"SELECT {storedName}", connection, transaction);

            foreach (var param in listParam)
            {
                if (param.Value == null)
                    param.Value = DBNull.Value;
                command.Parameters.Add(param);
            }

            command.CommandTimeout = timeOut;
            return command;
        }

        private NpgsqlBatchCommand GenBatchCommand(string storedName, List<object> listParam)
        {
            storedName = $"{storedName}({listParam.Select((x, index) => index + 1).Join()})";

            var command = new NpgsqlBatchCommand($"SELECT {storedName}");

            foreach (var param in listParam)
            {
                NpgsqlParameter npgsqlParameter = new NpgsqlParameter();
                npgsqlParameter.Value = param ?? DBNull.Value;
                command.Parameters.Add(npgsqlParameter);
            }

            return command;
        }

        public async Task ExecuteStoredNonQueryAsync(string storedName, List<object> listParam, int timeOut = 90)
        {
            await using var command = await GenCommandAsync(storedName, listParam, timeOut);
            await command.ExecuteNonQueryAsync();
        }


        public async Task ExecuteStoredNonQueryAsync(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90)
        {
            await using var command = await GenCommandAsync(storedName, listParam, timeOut);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<object?> ExecuteStoredScalarAsync(string storedName, List<object> listParam, int timeOut = 90)
        {
            await using var command = await GenCommandAsync(storedName, listParam, timeOut);

            var scalar = await command.ExecuteScalarAsync();
            return scalar;
        }

        public async Task<object?> ExecuteStoredScalarAsync(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90)
        {
            await using var command = await GenCommandAsync(storedName, listParam, timeOut);

            var scalar = await command.ExecuteScalarAsync();
            return scalar;
        }

        public async Task<string> ExecuteStoredStringAsync(string storedName, List<object> listParam, int timeOut = 90)
        {
            var scalar = await ExecuteStoredScalarAsync(storedName, listParam, timeOut);
            return scalar?.ToString() ?? "";
        }

        private async Task<NpgsqlDataReader> ExecuteStoredReaderAsync(string storedName, List<object> listParam, int timeOut = 90)
        {
            await using var command = await GenCommandAsync(storedName, listParam, timeOut);
            string? cursor = (string?)await command.ExecuteScalarAsync();

            await using var commandFetch = new NpgsqlCommand($"FETCH ALL IN \"{cursor}\"", connection, transaction);

            commandFetch.CommandTimeout = timeOut;

            return await commandFetch.ExecuteReaderAsync();
        }

        private async Task<NpgsqlDataReader> ExecuteStoredReaderAsync(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90)
        {
            await using var command = await GenCommandAsync(storedName, listParam, timeOut);
            string? cursor = (string?)await command.ExecuteScalarAsync();

            await using var commandFetch = new NpgsqlCommand($"FETCH ALL IN \"{cursor}\"", connection, transaction);

            commandFetch.CommandTimeout = timeOut;

            return await commandFetch.ExecuteReaderAsync();
        }

        public async Task<List<T>> ExecuteStoredListAsync<T>(string storedName, List<object> listParam, int timeOut = 90)
        {
            await using var reader = await ExecuteStoredReaderAsync(storedName, listParam, timeOut);
            return reader.Translate<T>();
        }

        public async Task<List<Dictionary<string, object>>> ExecuteStoredDictionaryAsync(string storedName, List<object> listParam, int timeOut = 90, bool isUpperCase = true)
        {
            await using var reader = await ExecuteStoredReaderAsync(storedName, listParam, timeOut);

            var results = new List<Dictionary<string, object>>();
            if (reader != null)
            {
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>(reader.FieldCount);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string colName = isUpperCase ? reader.GetName(i).ToUpper() : reader.GetName(i);
                        row[colName] = reader.GetValue(i);
                    }
                    results.Add(row);
                }
            }

            return results;
        }

        public async Task<DataTable> ExecuteStoredDataTableAsync(string storedName, List<object> listParam, int timeOut = 90, bool isUpperCase = true)
        {
            await using var reader = await ExecuteStoredReaderAsync(storedName, listParam, timeOut);
            var dataTable = new DataTable();
            dataTable.TableName = storedName;

            if (reader != null)
                dataTable.Load(reader);

            if (isUpperCase)
                dataTable.ToUpperColumnName();

            return dataTable;
        }

        public async Task ExecuteListStoredNoneQueryAsync(List<BatchCommandStoredModel> listCommand, int timeOut = 90)
        {
            await BeginTransactionAsync();

            await using var batch = new NpgsqlBatch(connection, transaction)
            {
                Timeout = timeOut
            };

            foreach (var command in listCommand)
            {
                NpgsqlBatchCommand npgsqlBatchCommand = GenBatchCommand(command.storedName, command.listParam);

                batch.BatchCommands.Add(npgsqlBatchCommand);
            }

            await batch.ExecuteNonQueryAsync();
        }

        public async Task<List<T>> ExecuteStoredListAsync<T>(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90)
        {
            await using var reader = await ExecuteStoredReaderAsync(storedName, listParam, timeOut);
            return reader.Translate<T>();
        }

        public async Task<string> ExecuteStoredStringAsync(string storedName, List<NpgsqlParameter> listParam, int timeOut = 90)
        {
            var scalar = await ExecuteStoredScalarAsync(storedName, listParam, timeOut);
            return scalar?.ToString() ?? "";
        }

    }
}
