using CardAPI.Domain.Entities.DTO;
using CardAPI.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace CardAPI.Application.Client.Queries.ClientData
{
    public record ClientData(string clientId) : IRequest<UserWithCardsDTO>;
    public class ClientDataQueryHandler : IRequestHandler<ClientData, UserWithCardsDTO>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogsRepository _logsRepository;

        public ClientDataQueryHandler(IClientRepository clientRepository, ILogsRepository logsRepository)
        {
            _clientRepository = clientRepository;
            _logsRepository = logsRepository;
        }

        public async Task<UserWithCardsDTO> Handle(ClientData request, CancellationToken cancellationToken)
        {
            var result = await _clientRepository.GetUserAndCard(request.clientId);
            return result;

        }
    }
}
