namespace OrdersManagement.Common.Helper
{
    public class ConfigHelper
    {
        public static AppSettingsModel configModel { get; set; }
    }
    public class AppSettingsModel
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string ConnectionString { get; set; }
        public string ConnectionStringStandby { get; set; }
        public string ConnectionStringForecast { get; set; }
        public string ConnectionStringReward { get; set; }
        public string ConnectionStringOmniBHX { get; set; }
        public string ConnectionStringBHXLogical { get; set; }
        public string ConnectionStringOld { get; set; }
        public string ConnectionStringStandbyOld { get; set; }
    }

}
