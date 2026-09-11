using AutoMapper;
using CardAPI.Application.Client.Queries.AccountBalance;
using CardAPI.Application.Client.Queries.ClientData;
using CardAPI.Domain.Entities.DAO;

namespace CardAPI.Utils.Profiles
{
    public class ClientProfile:Profile
    {
        public ClientProfile()
        {
            CreateMap<AccountBalanceRequest, GetAccountBalance>()
                .ForCtorParam("cardId", opt => opt.MapFrom(src => src.cardId))
                .ForCtorParam("userId", opt => opt.MapFrom(src => src.userId));


        }
    }
}
