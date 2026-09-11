using CardAPI.Domain.Entities.DTO;
using CardAPI.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace CardAPI.Application.Client.Queries.ClientData
{
    public record ClientData(string clientId) : IRequest<UserWithCardsDTO>;
    public record ClientDataByName(string clientName) : IRequest<UserWithCardsDTO>; 

    public class ClientDataQueryHandler : IRequestHandler<ClientData, UserWithCardsDTO>
    {
        private readonly IClientRepository _clientRepository;

        public ClientDataQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<UserWithCardsDTO> Handle(ClientData request, CancellationToken cancellationToken)
        {
            return await _clientRepository.GetUserAndCard(request.clientId);
        }
    }

    public class ClientDataByNameQueryHandler : IRequestHandler<ClientDataByName, UserWithCardsDTO>
    {
        private readonly IClientRepository _clientRepository;

        public ClientDataByNameQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<UserWithCardsDTO> Handle(ClientDataByName request, CancellationToken cancellationToken)
        {
            return await _clientRepository.GetUserByName(request.clientName);
        }
    }
}

