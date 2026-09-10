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
                var paymentIdParam = new SqlParameter("@ID_TARJETA", payment.IdCard);
                var paymentAmountParam = new SqlParameter("@MONTO", payment.Amount);
                var paymentDescriptionParam = new SqlParameter("@DESCRIPCION", payment.MvDescription);
                var paymentDateParam = new SqlParameter("@FECHA", payment.MvDate);
                var paymentCardParam = new SqlParameter("@PaymentId", System.Data.SqlDbType.Int)
                { Direction = System.Data.ParameterDirection.Output };

                var result = await _context.Database
                    .ExecuteSqlRawAsync("EXEC PROCESS_PAYMENT @ID_TARJETA, @MONTO, @DESCRIPCION, @FECHA, @PaymentId OUTPUT",
                    paymentIdParam, paymentAmountParam, paymentDescriptionParam, paymentDateParam, paymentCardParam);



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
               
                var purchaseIdTarjetaParam = new SqlParameter("@PurchaseIdTarjeta", purchase.IdCard);
                var purchaseAmountParam = new SqlParameter("@PurchaseAmount", purchase.Amount);
                var purchaseDescriptionParam = new SqlParameter("@PurchaseDescription", purchase.MvDescription);
                var purchaseDateParam = new SqlParameter("@PurchaseDate", purchase.MvDate);
                var purchaseOutStateParam = new SqlParameter("@Status", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var purchaseIdOutParam = new SqlParameter("@PurchaseId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };


                var result = await _context.PurchaseResults
                    .FromSqlRaw("EXEC PROCESS_PURCHASE @PurchaseIdTarjeta," +
                    " @PurchaseAmount, @PurchaseDescription, @PurchaseDate, @Status OUTPUT," +
                    " @PurchaseId OUTPUT", purchaseIdTarjetaParam, purchaseAmountParam, purchaseDescriptionParam, 
                    purchaseDateParam, purchaseOutStateParam, purchaseIdOutParam)
                    .ToListAsync();

                return result.FirstOrDefault();
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

        public async Task<AccountBalanceDTO> GetCardBalanceAsync(Guid cardId)
        {
            try
            {
                var cardIdParam = new SqlParameter("@ID_TARJETA", cardId);
                var result = await _context.AccountBalances
                    .FromSqlRaw("EXEC GET_ACCOUNT_BALANCE @ID_TARJETA", cardIdParam)
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
