using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;

namespace AccountDashboard.Web.Services.Interfaces
{
    /// <summary>Consume los endpoints de CardAPI.Controllers.ClientController.</summary>
    public interface IClientApiService
    {
        /// <summary>
        /// GET api/Client/{userId}.
        /// ASUNCION: se asume que devuelve la lista de tarjetas del cliente
        /// (ver nota en ApiContracts.ClientWithCardDto).
        /// </summary>
        Task<ApiResult<ClientWithCardDto>> GetClientCardsAsync(string userId);

        /// <summary>POST api/Client/Balance</summary>
        Task<ApiResult<AccountBalanceResponseDto>> GetAccountBalanceAsync(AccountBalanceRequestDto request);

        /// <summary>POST api/Client/TransactionHistory</summary>
        Task<ApiResult<List<TransactionHistoryLineDto>>> GetTransactionHistoryAsync(TransactionHistoryRequestDto request);
    }
}
