using AccountDashboard.Web.ApiContracts;
using AccountDashboard.Web.Common;

namespace AccountDashboard.Web.Services.Interfaces
{
    /// <summary>Consume los endpoints de CardAPI.Controllers.CardController.</summary>
    public interface ICardApiService
    {
        /// <summary>POST api/Card/purchase. El backend devuelve Ok()/BadRequest()/500 sin cuerpo tipado.</summary>
        Task<ApiResult> RegisterPurchaseAsync(PurchaseRequestDto request);

        /// <summary>POST api/Card/payment. El backend devuelve Ok()/BadRequest()/500 sin cuerpo tipado.</summary>
        Task<ApiResult> RegisterPaymentAsync(PaymentRequestDto request);
    }
}
