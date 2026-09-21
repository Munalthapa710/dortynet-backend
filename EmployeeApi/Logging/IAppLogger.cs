namespace EmployeeApi.Logging
{
    public interface IAppLogger
    {
        bool Log(LogType logType, Func<string> messageFunc, object? payload = null);

        bool CreateFile<T>();

        bool CreateFile(string name);
    }
}
