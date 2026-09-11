using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;
using AccountDashboard.Web.Services.Interfaces;

namespace AccountDashboard.Web.Services
{
    /// <inheritdoc cref="ICardApiService"/>
    public class CardApiService : ApiServiceBase, ICardApiService
    {
        public CardApiService(HttpClient httpClient, ILogger<CardApiService> logger)
            : base(httpClient, logger)
        {
        }

        public Task<ApiResult> RegisterPurchaseAsync(PurchaseRequestDto request)
        {
            return PostAsync("api/Card/purchase", request);
        }

        public Task<ApiResult> RegisterPaymentAsync(PaymentRequestDto request)
        {
            return PostAsync("api/Card/payment", request);
        }
    }
}
