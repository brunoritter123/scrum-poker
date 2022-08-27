using AutoMapper;
using Moq;
using ScrumPoker.Application.Configurations;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Services;
using ScrumPoker.Domain.Entities.Salas;
using ScrumPoker.Domain.Entities.Salas.Cartas;
using ScrumPoker.Domain.Entities.Salas.Configuracoes;
using ScrumPoker.Domain.Entities.Salas.Participantes;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ScrumPoker.Tests.Application.Services
{
    public class SalaServiceTests
    {
        private readonly IMapper _mapper;
        public SalaServiceTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperApiConfig());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact(DisplayName = "Buscar Sala por ID")]
        public async void BuscarPorIdAsyncTest()
        {
            // Arrange
            string id = "SalaID";
            var sala = SalaValida(id);

            var mockRepo = new Mock<ISalaRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync(sala);

            var subject = new SalaService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.ObterPorIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(sala.Participantes.Count(x => x.Jogador), result.Jogadores.Count());
            Assert.Equal(sala.Participantes.Count(x => !x.Jogador), result.Administradores.Count());
        }

        [Fact(DisplayName = "Buscar Sala por ID que não existe")]
        public async void BuscarPorIdAsync_OndeIdNaoExisteTest()
        {
            // Arrange
            string id = "SalaID";

            var mockRepo = new Mock<ISalaRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync((Sala)null);

            var subject = new SalaService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.ObterPorIdAsync(id);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(id.ToString(), exception.Message);
        }

        [Fact(DisplayName = "Resetar Jogo")]
        public async void ResetarJogoTest()
        {
            // Arrange
            string id = "SalaID";
            var sala = SalaValida(id);
            sala.JogoFinalizado = false;
            sala.UltimaDataDeUtilizacao = DateTime.MinValue;

            var mockRepo = new Mock<ISalaRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync(sala);

            var salaInputAlteracao = new Sala();
            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Sala>()))
                    .ReturnsAsync((Sala sala) => sala)
                    .Callback((Sala sala) => salaInputAlteracao = sala);

            var subject = new SalaService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.ResetarSala(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.False(result.JogoFinalizado);
            Assert.Null(result.Titulo);
            Assert.InRange(salaInputAlteracao.UltimaDataDeUtilizacao, DateTime.Now.AddMinutes(-5), DateTime.Now);
            Assert.DoesNotContain(result.Jogadores, x => x.VotoCartaValor != null);
            Assert.DoesNotContain(result.Administradores, x => x.VotoCartaValor != null);
        }

        [Fact(DisplayName = "Resetar Jogo de Sala que não existe")]
        public async void ResetarrJogoOndeSalaNaoExisteTest()
        {
            // Arrange
            string id = "SalaID";

            var mockRepo = new Mock<ISalaRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync((Sala)null);

            var subject = new SalaService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.ResetarSala(id);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(id.ToString(), exception.Message);
        }

        [Fact(DisplayName = "Finalizar Jogo")]
        public async void FinalizarJogoTest()
        {
            // Arrange
            string id = "SalaID";
            var sala = SalaValida(id);
            sala.JogoFinalizado = false;
            sala.UltimaDataDeUtilizacao = DateTime.MinValue;

            var mockRepo = new Mock<ISalaRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync(sala);

            var salaInputAlteracao = new Sala();
            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Sala>()))
                    .ReturnsAsync((Sala sala) => sala)
                    .Callback((Sala sala) => salaInputAlteracao = sala);

            var subject = new SalaService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.FinalizarJogo(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.True(result.JogoFinalizado);
            Assert.InRange(salaInputAlteracao.UltimaDataDeUtilizacao, DateTime.Now.AddMinutes(-5), DateTime.Now);
            Assert.Equal(sala.Titulo, result.Titulo);
        }

        [Fact(DisplayName = "Finalizar Jogo de Sala que não existe")]
        public async void FinalizarJogoOndeSalaNaoExisteTest()
        {
            // Arrange
            string id = "SalaID";

            var mockRepo = new Mock<ISalaRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync((Sala)null);

            var subject = new SalaService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.FinalizarJogo(id);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(id.ToString(), exception.Message);
        }

        [Fact(DisplayName = "Gerar Sala Padrão")]
        public async void GerarSalaPadraoTest()
        {
            // Arrange
            string id = "SalaID";
            var salaInput = SalaPadraoInputModelValida(id);
            salaInput.Configuracao.Cartas = new List<CartaViewModel>()
            {
                new CartaViewModel()
                {
                    Especial = true,
                    Ordem = 5,
                    Value = "05"
                }
            };

            var salaRetornoBd = SalaValida(id);
            salaRetornoBd.Configuracao.Cartas = new List<Carta>()
            {
                new Carta()
                {
                    Especial = false,
                    Ordem = 9,
                    Value = "09"
                }
            };

            var mockRepo = new Mock<ISalaRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync(salaRetornoBd);

            var salaInputAlteracao = new Sala();
            mockRepo.Setup(repo => repo.IncluirAsync(It.IsAny<Sala>()))
                    .ReturnsAsync((Sala sala) => sala)
                    .Callback((Sala sala) => salaInputAlteracao = sala);

            mockRepo.Setup(repo => repo.LimparSalasNaoUsadas());

            var subject = new SalaService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.GerarSalaPadraoAsync(salaInput);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(salaRetornoBd.Configuracao.Cartas.Count(), result.Configuracao.Cartas.Count());
            Assert.Equal(salaRetornoBd.Configuracao.Cartas.First().Ordem, result.Configuracao.Cartas.First().Ordem);
            Assert.Equal(salaRetornoBd.Configuracao.Cartas.First().Value, result.Configuracao.Cartas.First().Value);
            Assert.Equal(salaRetornoBd.Configuracao.Cartas.First().Especial, result.Configuracao.Cartas.First().Especial);
            mockRepo.Verify(mock => mock.LimparSalasNaoUsadas(), Times.Never);
        }

        [Fact(DisplayName = "Gerar Sala Padrao Quando Existe Sala Com Mesmo ID")]
        public async void GerarSalaPadraoQuandoExisteSalaComMesmoIDTest()
        {
            // Arrange
            string id = "SalaID";
            var salaInput = SalaPadraoInputModelValida(id);
            salaInput.Configuracao.Cartas = new List<CartaViewModel>()
            {
                new CartaViewModel()
                {
                    Especial = true,
                    Ordem = 5,
                    Value = "05"
                }
            };

            var mockRepo = new Mock<ISalaRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync((Sala)null);

            var salaInputInsert = new Sala();
            mockRepo.Setup(repo => repo.IncluirAsync(It.IsAny<Sala>()))
                    .ReturnsAsync((Sala sala) => sala)
                    .Callback((Sala sala) => salaInputInsert = sala);

            mockRepo.Setup(repo => repo.LimparSalasNaoUsadas());

            var subject = new SalaService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.GerarSalaPadraoAsync(salaInput);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.False(result.JogoFinalizado);
            Assert.InRange(salaInputInsert.UltimaDataDeUtilizacao, DateTime.Now.AddMinutes(-5), DateTime.Now);
            Assert.Equal(salaInput.Configuracao.Cartas.Count(), result.Configuracao.Cartas.Count());
            Assert.Equal(salaInput.Configuracao.Cartas.First().Ordem, result.Configuracao.Cartas.First().Ordem);
            Assert.Equal(salaInput.Configuracao.Cartas.First().Value, result.Configuracao.Cartas.First().Value);
            Assert.Equal(salaInput.Configuracao.Cartas.First().Especial, result.Configuracao.Cartas.First().Especial);
            mockRepo.Verify(mock => mock.LimparSalasNaoUsadas(), Times.Once);
        }

        private GerarSalaPadraoInputModel SalaPadraoInputModelValida(string id) => new GerarSalaPadraoInputModel()
        {
            Id = id,
            Configuracao = new SalaConfiguracaoViewModel()
            {
                Id = 1,
                SalaId = id,
                JogadorFinalizaJogo = false,
                JogadorResetaJogo = false,
                JogadorRemoveJogador = false,
                JogadorRemoveAdministrador = true,
                Cartas = new List<CartaViewModel>()
                {
                     new CartaViewModel()
                            {
                                Ordem = 1,
                                Value = "01",
                                Especial = false
                            },
                            new CartaViewModel()
                            {
                                Ordem = 2,
                                Value = "02",
                                Especial = false
                            },
                            new CartaViewModel()
                            {
                                Ordem = 3,
                                Value = "03",
                                Especial = false
                            }
                }
            }
        };

        private Sala SalaValida(string idSala)
        {
            var sala = new Sala()
            {
                Id = idSala,
                JogoFinalizado = false,
                UltimaDataDeUtilizacao = DateTime.Now,
                Titulo = "Titulo",
                Participantes = new List<Participante>()
                {
                    new Participante()
                    {
                        SalaId = idSala,
                        Id = "part1",
                        Jogador = true,
                        Nome = "teste",
                        VotoCartaValor = "02"
                    },
                    new Participante()
                    {
                        SalaId = idSala,
                        Id = "part2",
                        Jogador = true,
                        Nome = "teste2",
                        VotoCartaValor = "05"
                    },
                    new Participante()
                    {
                        SalaId = idSala,
                        Id = "part1",
                        Jogador = true,
                        Nome = "teste3",
                        VotoCartaValor = null
                    },
                    new Participante()
                    {
                        SalaId = idSala,
                        Id = "part1",
                        Jogador = false,
                        Nome = "teste4",
                        VotoCartaValor = null
                    }
                }
            };

            sala.Configuracao = new SalaConfiguracao()
            {
                SalaId = idSala,
                JogadorFinalizaJogo = false,
                JogadorResetaJogo = false,
                JogadorRemoveJogador = false,
                JogadorRemoveAdministrador = true,
                Cartas = new List<Carta>()
                        {
                            new Carta()
                            {
                                Ordem = 1,
                                Value = "01",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 2,
                                Value = "02",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 3,
                                Value = "03",
                                Especial = false
                            }
                        }
            };

            return sala;
        }
    }
}
