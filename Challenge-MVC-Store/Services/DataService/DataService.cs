using Challenge_MVC_Store.Services.LogService;

namespace Challenge_MVC_Store.Services.DataService
{
    public class DataService : IDataService
    {
        private readonly ILogService _logService;

        public DataService(ILogService logService)
        {
            _logService = logService;
        }

        public async Task ProcessDataAsync()
        {
            await Task.Run(() => _logService.Log("Mock processing..."));
        }
    }
}