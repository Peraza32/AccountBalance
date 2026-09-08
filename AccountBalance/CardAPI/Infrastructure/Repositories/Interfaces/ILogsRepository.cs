namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface ILogsRepository
    {
        public Task<object> InsertLogAsync(string origin, string description, DateTime logDate);
    }
}
