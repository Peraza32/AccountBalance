using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;
using AccountDashboard.Web.Services.Interfaces;

namespace AccountDashboard.Web.Services
{
    /// <inheritdoc cref="IClientApiService"/>
    public class ClientApiService : ApiServiceBase, IClientApiService
    {
        public ClientApiService(HttpClient httpClient, ILogger<ClientApiService> logger)
            : base(httpClient, logger)
        {
        }

        public Task<ApiResult<ClientWithCardDto>> GetClientCardsAsync(string userId)
        {
            // GET api/Client/{userId} -> ver nota de ASUNCION TEMPORAL en ClientWithCardDto.
            return GetAsync<ClientWithCardDto>($"api/Client/{Uri.EscapeDataString(userId)}");
        }

        public Task<ApiResult<AccountBalanceResponseDto>> GetAccountBalanceAsync(AccountBalanceRequestDto request)
        {
            return PostAsync<AccountBalanceRequestDto, AccountBalanceResponseDto>("api/Client/Balance", request);
        }

        public Task<ApiResult<List<TransactionHistoryLineDto>>> GetTransactionHistoryAsync(TransactionHistoryRequestDto request)
        {
            return PostAsync<TransactionHistoryRequestDto, List<TransactionHistoryLineDto>>(
                "api/Client/TransactionHistory", request);
        }
    }
}
