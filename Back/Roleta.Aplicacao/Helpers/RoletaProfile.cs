using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Dominio;
using Roleta.Dominio.Identity;
using AutoMapper;

namespace ActionCoins.Aplicacao.Helpers
{
    public class RoletaProfile : Profile
    {
        public RoletaProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<User, UserGameDto>().ReverseMap();
            CreateMap<UserDto, UserGameDto>().ReverseMap();
            CreateMap<User, UserDashBoardDto>().ReverseMap();
            CreateMap<UserDto, UserDashBoardDto>().ReverseMap();
            CreateMap<User, UserUpdateDashDto>().ReverseMap(); 
            CreateMap<RoletaSorte, RoletaSorteDto>().ReverseMap();
            CreateMap<Carteira, CarteiraDto>().ReverseMap();
            CreateMap<Transacao, TransacaoDto>().ReverseMap();
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<Pagamento, PagamentoDto>().ReverseMap();
            CreateMap<Saque, SaqueDto>().ReverseMap();
            //CreateMap<GiroRoleta, GiroRoletaDto>().ReverseMap();
        }
    }
}
