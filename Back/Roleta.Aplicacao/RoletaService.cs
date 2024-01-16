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


        public async Task<GiroRoletaDto> GirarRoleta(int valorAposta, bool freeSpin = false, UserGameDto? user = null)
        {
            try
            {
                List<ItemRoletaDto> posicoes = new List<ItemRoletaDto>()
                {
                    new ItemRoletaDto { Id = 1, Multiplicador = 0 },
                    new ItemRoletaDto { Id = 1, Multiplicador = 0 },
                    new ItemRoletaDto { Id = 2, Multiplicador = 0.5M },
                    new ItemRoletaDto { Id = 2, Multiplicador = 0.5M },
                    new ItemRoletaDto { Id = 3, Multiplicador = 1 },
                    new ItemRoletaDto { Id = 4, Multiplicador = 3 },
                    new ItemRoletaDto { Id = 5, Multiplicador = 0 },
                    new ItemRoletaDto { Id = 5, Multiplicador = 0 },
                    new ItemRoletaDto { Id = 6, Multiplicador = 0.5M },
                    new ItemRoletaDto { Id = 6, Multiplicador = 0.5M },
                    new ItemRoletaDto { Id = 7, Multiplicador = 1 },
                    new ItemRoletaDto { Id = 8, Multiplicador = 5 },
                    new ItemRoletaDto { Id = 9, Multiplicador = 100 },
                    new ItemRoletaDto { Id = 10, Multiplicador = 0 },
                    new ItemRoletaDto { Id = 10, Multiplicador = 0 },
                    new ItemRoletaDto { Id = 11, Multiplicador = 0.5M },
                    new ItemRoletaDto { Id = 11, Multiplicador = 0.5M },
                    new ItemRoletaDto { Id = 12, Multiplicador = 1 },
                    new ItemRoletaDto { Id = 13, Multiplicador = 7 },
                    new ItemRoletaDto { Id = 14, Multiplicador = 0 },
                    new ItemRoletaDto { Id = 14, Multiplicador = 0 },
                    new ItemRoletaDto { Id = 15, Multiplicador = 0.5M },
                    new ItemRoletaDto { Id = 15, Multiplicador = 0.5M },
                    new ItemRoletaDto { Id = 16, Multiplicador = 1 },
                    new ItemRoletaDto { Id = 17, Multiplicador = 9 },
                    new ItemRoletaDto { Id = 18, Multiplicador = 20 }
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

                var roleta = await GetByIdAsync(1);
                
                //Valor máximo que pode ser pago
                var valorPremiacaoMaxima = (roleta.SaldoBanca / 100) * roleta.PremiacaoMaxima;

                //se a contagem de perda for menor que taxa de perda ele zera
                if (roleta.ContagemPerda < roleta.TaxaPerda)
                    valorPremiacaoMaxima = 0;

                // Multiplicador maximo permitido para essa rodada
                //if (valorPremiacaoMaxima <= 0)
                //{
                //    if (user.Carteira.SaldoAtual < roleta.ValorMinimoSaque)
                //        valorPremiacaoMaxima = roleta.ValorMinimoSaque - user.Carteira.SaldoAtual;
                //}
                MaiorMultiplicadorPossivel = valorPremiacaoMaxima / valorAposta;

                if (MaiorMultiplicadorPossivel == 0) 
                    MaiorMultiplicadorPossivel = 1;

                posicoes = posicoes.FindAll(x => x.Multiplicador <= MaiorMultiplicadorPossivel);

                giro = Sorteio(posicoes, valorAposta);

                if (!await ProcessaGiro(giro, user, roleta))
                    throw new Exception("Erro ao processar o giro");

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

                double valorAposta = giro.ValorAposta;
                double valorPremio = valorAposta * (double)giro.Multiplicador;
                double valorPerda = valorAposta - valorPremio;

                //Debito do valor da aposta do Jogador
                var debitoUser = new Transacao()
                {
                    Tipo = "Giro Roleta",
                    valor = decimal.Negate((decimal)valorAposta),
                    Data = DateTime.Now,
                    CarteiraId = user.Carteira.Id
                };
                debitoUser = await _carteiraService.AddTransacaoAsync(debitoUser);
                user.Carteira.SaldoAtual += debitoUser.valor;

                if (valorPremio > 0)
                {
                    var debitoRoleta = new TransacaoRoleta()
                    {
                        valor = decimal.Negate((decimal)valorPremio),
                        Descricao = "Pagamento Giro",
                        Data = DateTime.Now,
                        RoletaId = roleta.Id,
                        TransacaoId = debitoUser.Id                        
                    };
                    debitoRoleta = await AddTransacaoRoleta(debitoRoleta);
                    roleta.SaldoBanca += debitoRoleta.valor;

                    var creditoPremio = new Transacao()
                    {
                        valor = (decimal)valorPremio,
                        Tipo = "Prêmio Roleta",
                        Data = DateTime.Now,
                        CarteiraId = user.Carteira.Id
                    };
                    creditoPremio = await _carteiraService.AddTransacaoAsync(creditoPremio);

                    user.Carteira.SaldoAtual += creditoPremio.valor;
                    roleta.ContagemPerda = 0;
                }

                //double valorBanca = valorAposta;

                if (valorPerda > 0)
                {
                    double valorBanca = valorPerda * ((double)roleta.PercentualBanca / 100);
                    valorPerda -= valorBanca;

                    //credito para a banca
                    var creditoBanca = new TransacaoRoleta()
                    {
                        valor = (decimal)valorBanca,
                        Descricao = "Divisao Banca",
                        Data = DateTime.Now,
                        TransacaoId = debitoUser.Id,
                        RoletaId = roleta.Id
                    };
                    creditoBanca = await AddTransacaoRoleta(creditoBanca);
                    roleta.SaldoBanca += creditoBanca.valor;


                    //verifica se tem afiliado e paga
                    if (user.ParentEmail != null)
                    {
                        var Afiliado = await _userService.GetUserGameByEmailAsync(user.ParentEmail);
                        if (Afiliado != null && Afiliado.isAfiliate && !Afiliado.isBlocked)
                        {
                            double comissaoAfiliado = valorPerda * ((double)Afiliado.Comissao / 100);

                            //credito para o afiliado
                            var creditoAfiliado = new Transacao()
                            {
                                Tipo = "Comissão",
                                valor = (decimal)comissaoAfiliado,
                                Data = DateTime.Now,
                                CarteiraId = Afiliado.Carteira.Id,
                                TransactionId = debitoUser.Id.ToString()
                            };
                            creditoAfiliado = await _carteiraService.AddTransacaoAsync(creditoAfiliado);
                            Afiliado.Carteira.SaldoAtual += creditoAfiliado.valor;

                            Afiliado = await _userService.UpdateUserGame(Afiliado);

                            if (Afiliado != null)
                                valorPerda -= comissaoAfiliado;
                        }
                    }

                    //credito para os lucros da casa
                    var creditoLucro = new TransacaoRoleta()
                    {
                        valor = (decimal)valorPerda,
                        Descricao = "Divisao Lucro",
                        Data = DateTime.Now,
                        TransacaoId = debitoUser.Id,
                        RoletaId = roleta.Id
                    };
                    creditoLucro = await AddTransacaoRoleta(creditoLucro);
                    roleta.SaldoLucro += creditoLucro.valor;
                    roleta.ContagemPerda++;
                }
                else
                {
                    //credito para banca
                    var creditoBanca = new TransacaoRoleta()
                    {
                        valor = (decimal)valorAposta,
                        Descricao = "Divisao Banca",
                        Data = DateTime.Now,
                        TransacaoId = debitoUser.Id,
                        RoletaId = roleta.Id
                    };
                    creditoBanca = await AddTransacaoRoleta(creditoBanca);
                    roleta.SaldoBanca += creditoBanca.valor;
                }

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
                    var retorno = await _roletaPersist.GetByIdAsync(roleta.Id);
                    return _mapper.Map<RoletaSorteDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoletaSorteDto> GetByIdAsync(int roletaId, bool includeTransacoes = false)
        {
            try
            {
                var roleta = await _roletaPersist.GetByIdAsync(roletaId, includeTransacoes);
                if (roleta == null) return null;

                var retorno = _mapper.Map<RoletaSorteDto>(roleta);
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task<GiroRoletaDto> AddAsync(GiroRoletaDto model)
        //{
        //    try
        //    {
        //        var giroRoleta = _mapper.Map<GiroRoleta>(model);

        //        _giroRoletaPersist.Add(giroRoleta);
        //        if (await _giroRoletaPersist.SaveChangeAsync())
        //        {
        //            var retorno = await _giroRoletaPersist.GetByIdAsync(giroRoleta.Id);
        //            return _mapper.Map<GiroRoletaDto>(retorno);
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public async Task<GiroRoletaDto> UpdateAsync(GiroRoletaDto model)
        //{
        //    try
        //    {
        //        var giroRoleta = await _giroRoletaPersist.GetByIdAsync(model.Id);
        //        if (giroRoleta == null) return null;

        //        _mapper.Map(model, giroRoleta);
        //        _giroRoletaPersist.Update(giroRoleta);
        //        if (await _giroRoletaPersist.SaveChangeAsync())
        //        {
        //            var retorno = await _giroRoletaPersist.GetByIdAsync(giroRoleta.Id);
        //            return _mapper.Map<GiroRoletaDto>(retorno);
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    try
        //    {
        //        var giroRoleta = await _giroRoletaPersist.GetByIdAsync(id);
        //        if (giroRoleta == null) throw new Exception("Giro da roleta não encontrado");

        //        _giroRoletaPersist.Delete(giroRoleta);
        //        return await _giroRoletaPersist.SaveChangeAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public async Task<GiroRoletaDto> GetGiroByIdAsync(int id)
        //{
        //    try
        //    {
        //        var geiroRoleta = await _giroRoletaPersist.GetByIdAsync(id);
        //        if (geiroRoleta == null) return null;

        //        return _mapper.Map<GiroRoletaDto>(geiroRoleta);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}
