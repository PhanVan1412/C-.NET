namespace OrdersManagement.Database
{
    public class Data
    {
        public Data()
        {
        }

        public enum DATABASETYPE
        {
            PosgreSQL
        }

        public class BatchCommandStoredModel
        {
            public BatchCommandStoredModel(string storedName, List<object> listParam)
            {
                this.storedName = storedName;
                this.listParam = listParam;
            }

            public string storedName { get; set; }
            public List<object> listParam { get; set; }
        }

        public static IData CreateData(String strConnect, DATABASETYPE dbtype = DATABASETYPE.PosgreSQL)
        {
            switch (dbtype)
            {
                case DATABASETYPE.PosgreSQL:
                    return new PosgreSQLData(strConnect);
                default:
                    throw new NotImplementedException("dbtype not found!");
            }
        }
    }
}
