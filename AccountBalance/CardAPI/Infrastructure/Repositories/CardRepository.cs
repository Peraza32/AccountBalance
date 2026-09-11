using CardAPI.Application.Client.Queries.AccountBalance;
using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;
using CardAPI.Infrastructure.Persistance;
using CardAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CardAPI.Infrastructure.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly CardDbContext _context;
        private readonly ILogsRepository _logsRepository;

        public CardRepository(CardDbContext context, ILogsRepository logsRepository)
        {
            _context = context;
            _logsRepository = logsRepository;
        }

        public async Task<PaymentResponseDTO> AddCardPayment(PaymentsTc payment)
        {
            try
            {
                var paymentCardParam = new SqlParameter("@ID_TARJETA", payment.IdCard);
                var paymentAmountParam = new SqlParameter("@MONTO", payment.Amount);
                var paymentDescriptionParam = new SqlParameter("@DESCRIPCION", payment.MvDescription);
                var paymentDateParam = new SqlParameter("@FECHA", payment.MvDate);
                var paymentIdParam = new SqlParameter("@PaymentId", System.Data.SqlDbType.Int)
                { Direction = System.Data.ParameterDirection.Output };

                var result = await _context.Database
                    .ExecuteSqlRawAsync("EXEC PROCESS_PAYMENT @ID_TARJETA, @MONTO, @DESCRIPCION, @FECHA, @PaymentId OUTPUT",
                    paymentCardParam, paymentAmountParam, paymentDescriptionParam, paymentDateParam, paymentIdParam);



                return new PaymentResponseDTO
                {
                    paymentId = (int)paymentIdParam.Value
                };

             }

            catch (Exception ex) {

                throw;
            }
            
        }

        public async Task<NewPurchaseResultDTO> AddCardPurchase(MovementsTc purchase)
        {
            
            try
            {
               
                var purchaseIdTarjetaParam = new SqlParameter("@ID_TARJETA", purchase.IdCard);
                var purchaseAmountParam = new SqlParameter("@MONTO", purchase.Amount);
                var purchaseDescriptionParam = new SqlParameter("@DESCRIPCION", purchase.MvDescription);
                var purchaseDateParam = new SqlParameter("@FECHA", purchase.MvDate);
                var purchaseOutStateParam = new SqlParameter("@Status", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var purchaseIdOutParam = new SqlParameter("@PurchaseId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };


                await _context.Database
                    .ExecuteSqlRawAsync("EXEC PROCESS_PURCHASE @ID_TARJETA," +
                    " @MONTO, @DESCRIPCION, @FECHA, @Status OUTPUT," +
                    " @PurchaseId OUTPUT", purchaseIdTarjetaParam, purchaseAmountParam, purchaseDescriptionParam, 
                    purchaseDateParam, purchaseOutStateParam, purchaseIdOutParam)
                    ;


                var result = new NewPurchaseResultDTO
                {
                    purchaseId = (int)purchaseIdOutParam.Value,
                    Status = (int)purchaseOutStateParam.Value
                };

                return result;
            }
            catch (Exception ex)
            {
                await _logsRepository.InsertLogAsync("CardRepository", ex.Message, DateTime.Now);
                throw;
            }

        }

        public Task<object> GetCardAsync(string cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountBalanceResponseDTO> GetCardBalanceAsync(GetAccountBalance request)
        {
            try
            {
                var cardIdParam = new SqlParameter("@ID_TARJETA", request.cardId);
                var userIdParam = new SqlParameter("@ID_CLIENTE", request.userId);
                var result = await _context.AccountBalances
                    .FromSqlRaw("EXEC GET_ACCOUNT_BALANCE @ID_TARJETA, @ID_CLIENTE", cardIdParam, userIdParam)
                    .ToListAsync();



                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        
    }
}
