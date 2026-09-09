using AutoMapper;
using CardAPI.Domain.Models;
using CardAPI.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace CardAPI.Application.Card.Command.Payment
{
    public record addPaymentCommand(Guid cardId, DateTime paymentDate, string description, decimal amount) : IRequest;
    public class AddPaymentCommandHandler : IRequestHandler<addPaymentCommand>
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

        public async Task Handle(addPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = _mapper.Map<addPaymentCommand, PaymentsTc>(request);
                payment.IdState = 1;
                await _cardRepository.AddCardPayment(payment);
            }
            catch (Exception ex)
            {
                await _logsRepository.InsertLogAsync("AddPaymentCommand", ex.Message, DateTime.Now);
                throw;
            }
        }
    }
}
