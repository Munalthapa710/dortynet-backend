namespace EmployeeApi.Logging
{
    public class DefaultLoggerSetting : ILoggerSetting
    {
        public DefaultLoggerSetting(IConfiguration configuration)
        {
            AllowLogging = configuration.GetValue("Logging:File:Enabled", true);
        }

        public bool AllowLogging { get; }
    }
}
