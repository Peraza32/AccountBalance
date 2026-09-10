
using CardAPI.Domain.Entities.DTO;
using CardAPI.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace CardAPI.Application.Client.Queries.AccountBalance
{
    public record GetAccountBalance(string cardId) : IRequest<AccountBalanceDTO>;
    public class AccountBalanceQueryHandler : IRequestHandler<GetAccountBalance, AccountBalanceDTO>
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
        public async Task<AccountBalanceDTO> Handle(GetAccountBalance request, CancellationToken cancellationToken)
        {
            try
            {

                var accountBalance = _cardRepository.GetCardBalanceAsync(Guid.Parse(request.cardId));
                var actualMonthPurchases = _purchaseRepository.GetPurchasesCurrentMonthAsync(request.cardId);

                await Task.WhenAll(accountBalance, actualMonthPurchases);
                var result = accountBalance.Result;
                
                if (accountBalance.Result == null)
                {
                    throw new Exception("Account balance not found.");
                }
                result.purchases = actualMonthPurchases.Result;
                return result;
            }
            catch (Exception ex) 
            {
                await _logsRepository.InsertLogAsync("AccountBalanceQueryHandler", ex.Message, DateTime.Now);
                throw new KeyNotFoundException("Account balance not found.");

            }
            
            
        }
    }
    
}

