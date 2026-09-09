namespace CardAPI.Infrastructure.Repositories.Interfaces
{
    public interface ILogsRepository
    {
        public Task InsertLogAsync(string origin, string description, DateTime logDate);
    }
}
