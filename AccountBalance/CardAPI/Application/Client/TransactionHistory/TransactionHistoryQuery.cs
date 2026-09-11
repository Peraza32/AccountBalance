using CardAPI.Domain.Entities.DTO;
using CardAPI.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace CardAPI.Application.Client.TransactionHistory
{
    public record GetTransactionHistory(Guid cardId, string userId)
        : IRequest<List<TransactionHistoryDTO>>;
    public class GetTransactionHistoryHandler 
        : IRequestHandler<GetTransactionHistory, List<TransactionHistoryDTO>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogsRepository _logsRepository;

        public GetTransactionHistoryHandler(
            IClientRepository clientRepository, 
            ILogsRepository logsRepository)
        {
            _clientRepository = clientRepository;
            _logsRepository = logsRepository;
        }

        public async Task<List<TransactionHistoryDTO>> Handle(
            GetTransactionHistory request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await _clientRepository.GetCurrentMonthTransactionsAsync(
                    request.cardId, request.userId);
            }
            catch (Exception ex)
            {
                await _logsRepository.InsertLogAsync(
                    "GetTransactionHistoryHandler", ex.Message, DateTime.UtcNow);
                throw;
            }
        }
    }
}
