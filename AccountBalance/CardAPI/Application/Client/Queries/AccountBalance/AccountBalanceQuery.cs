
using CardAPI.Domain.Entities.DTO;
using CardAPI.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace CardAPI.Application.Client.Queries.AccountBalance
{
    public record GetAccountBalance(Guid cardId, string userId) : IRequest<AccountBalanceResponseDTO>;
    public class AccountBalanceQueryHandler : IRequestHandler<GetAccountBalance, AccountBalanceResponseDTO>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ILogsRepository _logsRepository;
        public AccountBalanceQueryHandler(ICardRepository cardRepository, IPurchaseRepository purchaseRepository, ILogsRepository logsRepository)
        {
            _cardRepository = cardRepository;
            _purchaseRepository = purchaseRepository;
            _logsRepository = logsRepository;
        }
        public async Task<AccountBalanceResponseDTO> Handle(GetAccountBalance request, CancellationToken cancellationToken)
        {
            try
            {

                var accountBalance =
            await _cardRepository.GetCardBalanceAsync(request);

                if (accountBalance == null)
                {
                    throw new KeyNotFoundException(
                        "Account balance not found.");
                }

                var purchases =
                    await _purchaseRepository.GetPurchasesCurrentMonthAsync(
                        request.cardId.ToString());

                accountBalance.purchases = purchases;

                return accountBalance;
            }
            catch (Exception ex) 
            {
                await _logsRepository.InsertLogAsync("AccountBalanceQueryHandler", ex.Message, DateTime.Now);
                throw new KeyNotFoundException("Account balance not found.");

            }
            
            
        }
    }
    
}

