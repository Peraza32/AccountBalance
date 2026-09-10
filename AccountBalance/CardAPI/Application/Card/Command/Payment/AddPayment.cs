using AutoMapper;
using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;
using CardAPI.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace CardAPI.Application.Card.Command.Payment
{
    public record addPaymentCommand(Guid cardId, DateTime paymentDate, string description, decimal amount) : IRequest<PaymentResponseDTO>;
    public class AddPaymentCommandHandler : IRequestHandler<addPaymentCommand, PaymentResponseDTO>
    {
        private readonly ICardRepository _cardRepository;
        private readonly ILogsRepository _logsRepository;
        private readonly IMapper _mapper;

        public AddPaymentCommandHandler(ICardRepository cardRepository, ILogsRepository logsRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _logsRepository = logsRepository;
            _mapper = mapper;
        }

        public async Task<PaymentResponseDTO>  Handle(addPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = _mapper.Map<addPaymentCommand, PaymentsTc>(request);
                payment.IdState = 1;
                var result = await _cardRepository.AddCardPayment(payment);
                return result;
            }
            catch (Exception ex)
            {
                await _logsRepository.InsertLogAsync("AddPaymentCommand", ex.Message, DateTime.Now);
                throw;
            }
        }

        
    }
}
