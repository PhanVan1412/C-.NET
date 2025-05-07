using Npgsql;

namespace OrdersManagement.Database
{
    public class PostgreSQLService
    {
        #region -- Static (implement Singleton pattern) --

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile PostgreSQLService _instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static object _syncRoot = new Object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PostgreSQLService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new PostgreSQLService();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        public Dictionary<string, DataSource> dictDataSource { get; set; } = new Dictionary<string, DataSource>();

        public class DataSource
        {
            public DataSource(string connectionString, NpgsqlDataSource dataSource)
            {
                ConnectionString = connectionString;
                this.dataSource = dataSource;
            }

            public string ConnectionString { get; set; }
            public NpgsqlDataSource dataSource { get; set; }
        }

        public void AddConnection(string name, string connectionString, NpgsqlDataSource dataSource)
        {
            dictDataSource.Add(name, new DataSource(connectionString, dataSource));
        }
    }
}
