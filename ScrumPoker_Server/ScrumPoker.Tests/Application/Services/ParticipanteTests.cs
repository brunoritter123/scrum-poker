using AutoMapper;
using Moq;
using ScrumPoker.Application.Configurations;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Services;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScrumPoker.Tests.Application.Services
{
    public class ParticipanteTests
    {
        private readonly IMapper _mapper;
        public ParticipanteTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperApiConfig());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact(DisplayName = "Remover Participante")]
        public async void RemoverParticipanteTest()
        {
            // Arrange
            string participanteId = "teste123456";

            var mockRepo = new Mock<IParticipanteRepository>();

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            await subject.RemoverParticipante(participanteId);

            // Assert
            mockRepo.Verify(mock => mock.RemoverAsync(participanteId), Times.Once());
        }

        [Fact(DisplayName = "Buscar Participante Por Id")]
        public async void BuscarParticipantePorIdTest()
        {
            // Arrange
            string participanteId = "teste123456";
            var participanteBd = ParticipanteValido();
            participanteBd.Id = participanteId;

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync(participanteBd);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.BuscarParticipantePorId(participanteId);

            // Assert
            mockRepo.Verify(mock => mock.BuscarPorIdAsync(participanteId), Times.Once());
            Assert.IsType<ParticipanteViewModel>(result);
            Assert.Equal(participanteId, result.Id);
        }

        [Fact(DisplayName = "Buscar Participante Por Id Que Não Existe")]
        public async void BuscarParticipantePorIdQueNaoExisteTest()
        {
            // Arrange
            string participanteId = "teste123456";

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync((Participante)null);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.BuscarParticipantePorId(participanteId);

            // Assert
            mockRepo.Verify(mock => mock.BuscarPorIdAsync(participanteId), Times.Once());
            Assert.Null(result);
        }

        [Fact(DisplayName = "Voto Participante")]
        public async void VotoParticipanteTest()
        {
            // Arrange
            string participanteId = "teste123456";
            string votoDoParticipante = "11";
            var participanteBd = ParticipanteValido();
            participanteBd.Id = participanteId;
            participanteBd.VotoCartaValor = "03";

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync(participanteBd);

            var participanteReturnoAlteracao = new Participante();
            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Participante>()))
                    .ReturnsAsync((Participante part) => part);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.VotoParticipante(participanteId, votoDoParticipante);

            // Assert
            Assert.Equal(participanteId, result.JogadorId);
            Assert.Equal(votoDoParticipante, result.ValorVoto);
            Assert.Equal(participanteBd.SalaId, result.SalaId);
        }

        [Fact(DisplayName = "Voto de Participante que não existe")]
        public async void VotoParticipanteQueNaoExisteTest()
        {
            // Arrange
            string participanteId = "teste123456";
            string votoDoParticipante = "11";
            var participanteBd = ParticipanteValido();
            participanteBd.Id = participanteId;
            participanteBd.VotoCartaValor = "03";

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync((Participante)null);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.VotoParticipante(participanteId, votoDoParticipante);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(participanteId, exception.Message);
        }

        [Fact(DisplayName = "Conectar Participante")]
        public async void ConectarTest()
        {
            // Arrange
            string participanteId = "teste123456";
            string conexaoId = "11testete";
            var participanteBd = ParticipanteValido();
            participanteBd.Id = participanteId;
            participanteBd.VotoCartaValor = "03";

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync(participanteBd);

            var participanteReturnoAlteracao = new Participante();
            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Participante>()))
                    .ReturnsAsync((Participante part) => part)
                    .Callback((Participante part) => participanteReturnoAlteracao = part);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.Conectar(conexaoId, participanteId);

            // Assert
            Assert.Equal(participanteId, result.Id);
            Assert.Equal(conexaoId, participanteReturnoAlteracao.ConexaoId);
            Assert.True(result.Online);
        }

        [Fact(DisplayName = "Conectar Participante que não existe")]
        public async void ConectarParticipanteQueNaoExisteTest()
        {
            // Arrange
            string participanteId = "teste123456";
            string conexaoId = "11testete";
            var participanteBd = ParticipanteValido();
            participanteBd.Id = participanteId;
            participanteBd.VotoCartaValor = "03";

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync((Participante)null);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.Conectar(conexaoId, participanteId);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(participanteId, exception.Message);
        }

        private Participante ParticipanteValido()
        {
            return new Participante()
            {
                Id = "participante123",
                SalaId = "teste123",
                ConexaoId = "conexao123",
                Nome = "Bruno",
                Jogador = true,
                Online = true,
                VotoCartaValor = "03"
            };
        }
        //[Theory]
        //[InlineData("1213asdf", "SalaTeste", "Participante1", true)]
        //public async void IncluirParticipanteEmUmaSalaAsync_NovaSalaEParticipante
        //    (string id, string salaId, string nome, bool jogador)
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IParticipanteGateway>();
        //    mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
        //        .ThrowsAsync(new EntidadeNaoEncontradaException(nameof(Participante), id));
        //    mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Participante>()));
        //    mockRepo.Setup(repo => repo.IncluirAsync(It.IsAny<Participante>()));

        //    var mockRepoSala = new Mock<ISalaGateway>();
        //    mockRepoSala.Setup(repoSala => repoSala.NaoExisteSalaAsync(salaId))
        //        .Returns(Task.FromResult(false));
        //    mockRepoSala.Setup(repoSala => repoSala.IncluirAsync(It.IsAny<Sala>()))
        //        .Returns(Task.FromResult<Sala>(null));


        //    var act = new ParticipanteUserCase(mockRepo.Object, mockRepoSala.Object);

        //    // Act
        //    var result = await act.IncluirParticipanteEmUmaSalaAsync(id, salaId, nome, jogador);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(id, result.Id);
        //    Assert.Equal(salaId, result.SalaId);
        //    Assert.Equal(nome, result.Nome);
        //    Assert.Equal(jogador, result.Jogador);
        //}

        //[Theory]
        //[InlineData("1213asdf", "SalaTeste", "Participante1", true)]
        //public async void IncluirParticipanteEmUmaSalaAsync_NovaSalaEParticipanteExistente
        //    (string id, string salaId, string nome, bool jogador)
        //{
        //    // Arrange
        //    var participanteExistente = new Participante(id, salaId, nome, jogador);
        //    participanteExistente.Online = false;
        //    participanteExistente.VotoCartaValor = "100";

        //    var mockRepo = new Mock<IParticipanteGateway>();
        //    mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
        //        .ThrowsAsync(new EntidadeNaoEncontradaException(nameof(Participante), id));
        //    mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Participante>()));
        //    mockRepo.Setup(repo => repo.IncluirAsync(It.IsAny<Participante>()));

        //    var mockRepoSala = new Mock<ISalaGateway>();
        //    mockRepoSala.Setup(repoSala => repoSala.NaoExisteSalaAsync(salaId))
        //        .Returns(Task.FromResult(false));
        //    mockRepoSala.Setup(repoSala => repoSala.IncluirAsync(It.IsAny<Sala>()))
        //        .Returns(Task.FromResult<Sala>(null));


        //    var act = new ParticipanteUserCase(mockRepo.Object, mockRepoSala.Object);

        //    // Act
        //    var result = await act.IncluirParticipanteEmUmaSalaAsync(id, salaId, nome, jogador);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(id, result.Id);
        //    Assert.Equal(salaId, result.SalaId);
        //    Assert.Equal(nome, result.Nome);
        //    Assert.Equal(jogador, result.Jogador);
        //}
    }
}