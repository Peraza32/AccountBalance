using AutoMapper;
using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;
using CardAPI.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace CardAPI.Application.Card.Command.Purchase
{
    public record AddPurchaseCommand(Guid cardId, DateTime purchaseDate, string description, decimal price) : IRequest<NewPurchaseResultDTO> {}
    public class AddPurchaseCommandHandler : IRequestHandler<AddPurchaseCommand,NewPurchaseResultDTO>
    {
        private readonly ICardRepository _cardRepository;
        private readonly ILogsRepository _logsRepository;
        private readonly IMapper _mapper;

        public AddPurchaseCommandHandler(ICardRepository cardRepository, ILogsRepository logsRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _logsRepository = logsRepository;
            _mapper = mapper;
        }


        public async Task<NewPurchaseResultDTO>  Handle(AddPurchaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var purchase = _mapper.Map<AddPurchaseCommand, MovementsTc>(request);
                purchase.IdState = 1;

                var result = await _cardRepository.AddCardPurchase(purchase);

                
                if (result is null)
                    throw new ApplicationException("No se pudo procesar la compra.");
                return result;
            }
            catch (Exception ex)
            {
                await _logsRepository.InsertLogAsync("AddPurchaseCommand", ex.Message, DateTime.Now);
                throw;
            }
        }
    }
}
