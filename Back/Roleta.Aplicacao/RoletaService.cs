using AutoMapper;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Aplicacao
{
    public class RoletaService : IRoletaService
    {
        private readonly IAccountService _accountService;
        private readonly IGiroRoletaPersist _giroRoletaPersist;
        private readonly IMapper _mapper;

        public RoletaService(IAccountService accountService, IGiroRoletaPersist giroRoletaPersist, IMapper mapper)
        {
            _accountService = accountService;
            _giroRoletaPersist = giroRoletaPersist;
            _mapper = mapper;
        }


        public async Task<GiroRoletaDto> GirarRoleta(int valorAposta, bool freeSpin = false, UserDto? user = null)
        {
            try
            {
                int[] posicoes;
                int posicaoSorteada;
                decimal multiplicador;
                UserGameDto userGame;
                //1 = 0
                //2 = 0.5m
                //3 = 1.2m
                //4 = 3
                //5 = 0
                //6 = 0.5m
                //7 = 1.2m
                //8 = 5
                //9 = 100
                //10 = 0
                //11 = 0.5m
                //12 = 1.2m
                //13 = 7
                //14 = 0
                //15 = 0.5m
                //16 = 1.2m
                //17 = 9
                //18 = 30


                if (freeSpin)
                {
                    //0.5 tem 4 posicoes
                    posicoes = new int[] { 1, 2, 3, 4, 4, 7, 8, 8, 9, 9, 10, 11, 13, 13, 14, 15, 16, 17, 18, 18 };
                    //posicoes = new decimal[] { 0.5m, 1, 1.2m, 3, 0.5m, 1, 1.2m, 5, 100, 0.5m, 1, 1.2m, 7, 0.5m, 1, 1.2m, 9, 30 };
                }
                else
                {
                    if (user.DemoAcount)
                    {
                        posicoes = new int[] { 1, 2, 3, 4, 4, 7, 8, 8, 9, 9, 10, 11, 13, 13, 14, 15, 16, 17, 18, 18 };
                    }
                    else
                    {
                        //1° passo - verificar o multiplicador e saber o valor maximo que pode ter do sorteio
                        //2° passo - verificar saldo da casa e ver se está acima de 5k e o valor tem que ser mair que 10% da banca   
                        //3° passo - se sorteio for vencedor, entao atualizar o saldo total de deposito e o saldo do usuario
                        //posicoes = new decimal[] { 0.5m, 1, 1.2m, 3, 0.5m, 1, 1.2m, 5, 100, 0.5m, 1, 1.2m, 7, 0.5m, 1, 1.2m, 9, 30 };
                        //posicoes = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
                        switch (valorAposta)
                        {
                            case 1:
                                posicoes = new int[] { 1, 1, 2, 2, 3, 3, 4, 5, 5, 7, 8, 10, 11, 11, 13, 14, 14, 15, 16, 17 };
                                break;
                            case 50:
                                posicoes = new int[] { 1, 1, 1, 2, 2, 2, 3, 5, 5, 5, 6, 6, 7, 10, 10, 10, 11, 11, 12, 14, 14, 14, 15, 15 };
                                break;
                            default:
                                posicoes = new int[] { 1, 1, 1, 2, 2, 2, 3, 5, 5, 5, 6, 6, 7, 10, 10, 10, 11, 11, 12, 14, 14, 14, 15, 15 };
                                //posicoes = new int[] { 1, 1, 2, 2, 3, 5, 6, 6, 7, 10, 11, 11, 12, 14, 15 };
                                break;
                        }
                    }                    
                }

                var rnd = new Random();
                var r = rnd.Next(posicoes.Length);
                posicaoSorteada = (posicoes[r]);

                switch (posicaoSorteada)
                {
                    case (1): multiplicador = 0; break;
                    case (2): multiplicador = 0.5m; break;
                    case (3): multiplicador = 1.2m; break;
                    case (4): multiplicador = 3; break;
                    case (5): multiplicador = 0; break;
                    case (6): multiplicador = 0.5m; break;
                    case (7): multiplicador = 1.2m; break;
                    case (8): multiplicador = 5; break;
                    case (9): multiplicador = 100; break;
                    case (10): multiplicador = 0; break;
                    case (11): multiplicador = 0.5m; break;
                    case (12): multiplicador = 1.2m; break;
                    case (13): multiplicador = 7m; break;
                    case (14): multiplicador = 0; break;
                    case (15): multiplicador = 0.5m; break;
                    case (16): multiplicador = 1.2m; break;
                    case (17): multiplicador = 9; break;
                    case (18): multiplicador = 30; break;
                    default: multiplicador = 0; break;
                }

                GiroRoletaDto giro = new GiroRoletaDto()
                {
                    ValorAposta = valorAposta,
                    Posicao = posicaoSorteada,
                    Multiplicador = multiplicador,
                    UserId = user?.Id
                };

                if (!freeSpin)
                {
                    giro = await AddAsync(giro);
                    //giro.Posicao = posicaoSorteada;
                }

                return giro;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GiroRoletaDto> AddAsync(GiroRoletaDto model)
        {
            try
            {
                var giroRoleta = _mapper.Map<GiroRoleta>(model);

                _giroRoletaPersist.Add(giroRoleta);
                if (await _giroRoletaPersist.SaveChangeAsync())
                {
                    var retorno = await _giroRoletaPersist.GetByIdAsync(giroRoleta.Id);
                    return _mapper.Map<GiroRoletaDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GiroRoletaDto> UpdateAsync(GiroRoletaDto model)
        {
            try
            {
                var giroRoleta = await _giroRoletaPersist.GetByIdAsync(model.Id);
                if (giroRoleta == null) return null;

                _mapper.Map(model, giroRoleta);
                _giroRoletaPersist.Update(giroRoleta);
                if (await _giroRoletaPersist.SaveChangeAsync())
                {
                    var retorno = await _giroRoletaPersist.GetByIdAsync(giroRoleta.Id);
                    return _mapper.Map<GiroRoletaDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var giroRoleta = await _giroRoletaPersist.GetByIdAsync(id);
                if (giroRoleta == null) throw new Exception("Giro da roleta não encontrado");

                _giroRoletaPersist.Delete(giroRoleta);
                return await _giroRoletaPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GiroRoletaDto> GetByIdAsync(int id)
        {
            try
            {
                var geiroRoleta = await _giroRoletaPersist.GetByIdAsync(id);
                if (geiroRoleta == null) return null;

                return _mapper.Map<GiroRoletaDto>(geiroRoleta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
