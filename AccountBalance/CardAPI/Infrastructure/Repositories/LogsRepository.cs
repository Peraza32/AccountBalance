using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;
using CardAPI.Infrastructure.Persistance;
using CardAPI.Infrastructure.Repositories.Interfaces;

namespace CardAPI.Infrastructure.Repositories
{
    public class LogsRepository : ILogsRepository
    {
        private readonly CardDbContext _context;

        public LogsRepository(CardDbContext context)
        {
            _context = context;
        }

        public async  Task InsertLogAsync(string origin, string description, DateTime logDate)
        {
            var log = new Log
            {
                Origin = origin,
                LogDescription = description,
                LogDt = logDate
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
