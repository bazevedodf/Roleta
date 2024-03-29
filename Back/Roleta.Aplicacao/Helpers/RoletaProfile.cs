﻿using Roleta.Aplicacao.Dtos;
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
            CreateMap<User, AfiliadoDto>().ReverseMap();
            CreateMap<UserDto, UserGameDto>().ReverseMap();
            CreateMap<User, UserDashBoardDto>().ReverseMap();
            CreateMap<UserDto, UserDashBoardDto>().ReverseMap();
            CreateMap<User, UserUpdateDashDto>().ReverseMap();
            CreateMap<User, UpdateUserGameDto>().ReverseMap();
            CreateMap<RoletaSorte, RoletaSorteDto>().ReverseMap(); 
            CreateMap<RoletaSorte, RoletaSorteUpdateDto>().ReverseMap();
            CreateMap<RoletaSorteDto, RoletaSorteUpdateDto>().ReverseMap();
            CreateMap<BancaPagadora, BancaPagadoraDto>().ReverseMap();
            CreateMap<Carteira, CarteiraDto>().ReverseMap();
            CreateMap<Transacao, TransacaoDto>().ReverseMap();
            CreateMap<_Produto, ProdutoDto>().ReverseMap();
            CreateMap<Pagamento, PagamentoDto>().ReverseMap();
            CreateMap<Saque, SaqueDto>().ReverseMap();
            //CreateMap<GiroRoleta, GiroRoletaDto>().ReverseMap();
        }
    }
}
