using AutoMapper;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Aplicacao
{
    public class RoletaService : IRoletaService
    {
        private readonly IRoletaPersist _roletaPersist;
        private readonly ITransacaoRoletaPersist _transacaoRoletaPersist;
        private readonly ICarteiraService _carteiraService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public RoletaService(IRoletaPersist roletaPersist,
                             ITransacaoRoletaPersist transacaoRoletaPersist,
                             ICarteiraService carteiraService,
                             IUserService userService,
                             IMapper mapper)
        {
            _roletaPersist = roletaPersist;
            _transacaoRoletaPersist = transacaoRoletaPersist;
            _carteiraService = carteiraService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<bool> DepositoCaixa(PagamentoDto pagamento, int roletaId)
        {
            try
            {
                var roleta = await GetByIdAsync(roletaId, true);
                
                if (roleta != null) 
                {
                    var desconto = pagamento.Valor * 0.03M;
                    roleta.SaldoLucro += pagamento.Valor - desconto;

                    //Criar e alimentar o caixa do proximo dia
                    if (roleta.BancasPagadoras.Count() == 0)
                    {
                        List<BancaPagadora> bancas = new() { 
                            new BancaPagadora()
                            {
                                RoletaSorteId = roleta.Id,
                                SaldoDia = 0,
                                DataBanca = DateTime.Now.AddDays(1)
                            }
                        };

                        roleta.BancasPagadoras = bancas;
                    }

                    decimal valorBanca = pagamento.Valor * ((decimal)roleta.PercentualBanca / 100);
                    var banca = roleta.BancasPagadoras.FirstOrDefault(x => x.DataBanca.Date > DateTime.Now.Date);
                    banca.SaldoDia += valorBanca;

                    List<OperacaoRoleta> operacoes = new() {
                            new OperacaoRoleta()
                            {
                                Descricao = "Depósito",
                                Valor = pagamento.Valor - desconto,
                                UserId = pagamento.UserId,
                                RoletaSorteId = roleta.Id,
                                TransactionId = pagamento.TransactionId
                            }
                        };
                    roleta.OperacoesRoleta = operacoes;

                    var result = await UpdateAsync(roleta);

                    if (result != null) return true;
                }
                
                return false;
            }
            catch (Exception)
            {
                throw new Exception("Erro ao tentar fazer o deposito no caixa da roleta.");
            }
        }

        public async Task<bool> SaqueCaixa(decimal valorSaque, int roletaId)
        {
            try
            {
                var roleta = await GetByIdAsync(roletaId);

                if (roleta != null)
                {
                                    
                }

                return false;
            }
            catch (Exception)
            {
                throw new Exception("Erro ao tentar fazer o saque no caixa da roleta.");
            }
        }

        public async Task<bool> ComissaoAfiliado(string emailAfiliado, decimal valorDeposito, string transacionId)
        {
            try
            {
                var user = await _userService.GetByUserLoginAsync(emailAfiliado);
                if (user != null && !user.isBlocked)
                {
                    user.Carteira.SaldoAtual += user.ValorComissao;
                    var retorno = await _userService.UpdateUserDashBoard(user);
                    if (retorno != null) return true;

                    var creditoAfiliado = new Transacao()
                    {
                        Tipo = "Comissão",
                        valor = user.ValorComissao,
                        Data = DateTime.Now,
                        CarteiraId = user.Carteira.Id,
                        TransactionId = transacionId
                    };
                    await _carteiraService.AddTransacaoAsync(creditoAfiliado);
                }

                return false;
            }
            catch (Exception)
            {
                throw new Exception("Erro ao tentar distribuir a comissão do afiliado.");
            }
        }

        public async Task<GiroRoletaDto> GirarRoleta(int valorAposta, bool freeSpin = false, UserGameDto? user = null)
        {
            try
            {
                List<ItemRoletaDto> posicoes = new List<ItemRoletaDto>()
                {
                    new ItemRoletaDto { Id = 1, Multiplicador = 0, Peso = 36 },
                    new ItemRoletaDto { Id = 2, Multiplicador = 0.5M, Peso = 35 },
                    new ItemRoletaDto { Id = 3, Multiplicador = 1, Peso = 15 },
                    new ItemRoletaDto { Id = 4, Multiplicador = 3, Peso = 4 },
                    new ItemRoletaDto { Id = 5, Multiplicador = 0, Peso = 36},
                    new ItemRoletaDto { Id = 6, Multiplicador = 0.5M, Peso = 35 },
                    new ItemRoletaDto { Id = 7, Multiplicador = 1, Peso = 15 },
                    new ItemRoletaDto { Id = 8, Multiplicador = 5, Peso = 4 },
                    new ItemRoletaDto { Id = 9, Multiplicador = 100, Peso = 1 },
                    new ItemRoletaDto { Id = 10, Multiplicador = 0, Peso = 36},
                    new ItemRoletaDto { Id = 11, Multiplicador = 0.5M, Peso = 35 },
                    new ItemRoletaDto { Id = 12, Multiplicador = 1, Peso = 15 },
                    new ItemRoletaDto { Id = 13, Multiplicador = 7, Peso = 2 },
                    new ItemRoletaDto { Id = 14, Multiplicador = 0, Peso = 36},
                    new ItemRoletaDto { Id = 15, Multiplicador = 0.5M, Peso = 35 },
                    new ItemRoletaDto { Id = 16, Multiplicador = 1, Peso = 15 },
                    new ItemRoletaDto { Id = 17, Multiplicador = 9, Peso = 2 },
                    new ItemRoletaDto { Id = 18, Multiplicador = 20, Peso = 1 }
                };

                GiroRoletaDto giro;
                decimal MaiorMultiplicadorPossivel = 0;

                if (freeSpin)
                {
                    posicoes = posicoes.DistinctBy(x => x.Id).ToList();
                    giro = Sorteio(posicoes, valorAposta);
                    return giro;
                }
                
                if (user.DemoAcount)
                {
                    posicoes = posicoes.DistinctBy(x => x.Id).ToList();
                    posicoes = posicoes.FindAll(x => x.Multiplicador > 1);
                    giro = Sorteio(posicoes, valorAposta);
                    
                    if (!await ProcessaGiro(giro, user))
                        throw new Exception("Erro ao processar o giro");
                    
                    return giro;
                }

                var roleta = await GetByIdAsync(1, true);
                
                BancaPagadora banca = roleta.BancasPagadoras.FirstOrDefault();

                //Valor máximo que pode ser pago
                var valorPremiacaoMaxima = (banca.SaldoDia / 100) * roleta.PremiacaoMaxima;

                //Multiplicador maximo permitido para essa rodada
                if (valorPremiacaoMaxima <= 0)
                {
                    // coloca a chance de ganhar até o valor do saque minimo
                    if (user.Carteira.SaldoAtual < roleta.ValorMinimoSaque)
                        valorPremiacaoMaxima = roleta.ValorMinimoSaque - user.Carteira.SaldoAtual;
                }

                MaiorMultiplicadorPossivel = valorPremiacaoMaxima / valorAposta;

                posicoes = posicoes.FindAll(x => x.Multiplicador <= MaiorMultiplicadorPossivel);

                giro = SorteioCumulativo(posicoes, valorAposta);

                //sortear uma posicao com o multiplicador selecionado
                Random rnd = new Random();
                var item = posicoes.Where(x => x.Multiplicador == giro.Multiplicador)
                                   .OrderBy(x => rnd.Next())
                                   .First();

                giro.Posicao = item.Id;

                //if (!await ProcessaGiro(giro, user, roleta))
                //    throw new Exception("Erro ao processar o giro");

                return giro;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<bool> ProcessaGiro(GiroRoletaDto giro, UserGameDto user, RoletaSorteDto roleta = null)
        {
            try
            {
                if (user.DemoAcount)
                {
                    if (giro.Multiplicador > 0)
                    {
                        user.Carteira.SaldoDemo -= giro.ValorAposta;
                        user.Carteira.SaldoDemo += giro.ValorAposta * giro.Multiplicador;
                        await _userService.UpdateUserGame(user);
                    }
                    return true;
                }

                decimal valorPremio = giro.ValorAposta * giro.Multiplicador;

                user.Carteira.SaldoAtual += valorPremio - giro.ValorAposta;
                user.Carteira.DataAtualizacao = DateTime.Now;
                user = await _userService.UpdateUserGame(user);

                roleta = await UpdateAsync(roleta);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private GiroRoletaDto Sorteio(List<ItemRoletaDto> posicoes, int valorAposta)
        {
            var rnd = new Random();
            var r = rnd.Next(0,posicoes.Count);
            var posicaoSorteada = posicoes[r];

            return new GiroRoletaDto()
            {
                ValorAposta = valorAposta,
                Posicao = posicaoSorteada.Id,
                Multiplicador = posicaoSorteada.Multiplicador,
            };
        }

        public GiroRoletaDto SorteioCumulativo(List<ItemRoletaDto> posicoes, int valorAposta)
        {
            var itens = posicoes.DistinctBy(x => x.Multiplicador).OrderBy(x => x.Multiplicador).ToList();
            var pesoTotal = (int)itens.Sum(x => x.Peso);
            Random r = new Random();
            decimal numSortiado = r.Next(0, pesoTotal + 1);

            decimal acumulativo = 0;
            ItemRoletaDto posicaoSorteada = itens[0];

            for (int i = 0; i < itens.Count; i++)
            {
                acumulativo += itens[i].Peso;
                if (numSortiado <= acumulativo)
                {
                    posicaoSorteada = itens[i];
                    break;
                }
            }
            return new GiroRoletaDto()
            {
                ValorAposta = valorAposta,
                Posicao = posicaoSorteada.Id,
                Multiplicador = posicaoSorteada.Multiplicador,
            };
        }

        private async Task<TransacaoRoleta> AddTransacaoRoleta(TransacaoRoleta transacao)
        {
            try
            {
                _transacaoRoletaPersist.Add(transacao);
                if (await _transacaoRoletaPersist.SaveChangeAsync())
                {
                    var retorno = await _transacaoRoletaPersist.GetByIdAsync(transacao.Id);
                    return retorno;
                }
                return null;
            }
            catch (Exception)
            {
                throw new Exception("Erro na Transação da Roleta");
            }
        }

        public async Task<RoletaSorteDto> UpdateAsync(RoletaSorteDto model)
        {
            try
            {
                var roleta = await _roletaPersist.GetByIdAsync(model.Id);
                if (roleta == null) return null;

                _mapper.Map(model, roleta);

                _roletaPersist.Update(roleta);
                if (await _roletaPersist.SaveChangeAsync())
                {
                    var retorno = await _roletaPersist.GetByIdAsync(roleta.Id, true);
                    return _mapper.Map<RoletaSorteDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoletaSorteDto> GetByIdAsync(int roletaId, bool includeBancaDia = false, bool includeTransacoes = false)
        {
            try
            {
                var roleta = await _roletaPersist.GetByIdAsync(roletaId, includeBancaDia, includeTransacoes);
                if (roleta == null) return null;

                var retorno = _mapper.Map<RoletaSorteDto>(roleta);
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
