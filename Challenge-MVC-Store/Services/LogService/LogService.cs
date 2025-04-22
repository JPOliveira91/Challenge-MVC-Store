namespace Challenge_MVC_Store.Services.LogService
{
    public class LogService : ILogService
    {
        public void Log(string message)
        {
            Console.WriteLine($"Log - {DateTime.Now} - {message}");
        }
    }
}