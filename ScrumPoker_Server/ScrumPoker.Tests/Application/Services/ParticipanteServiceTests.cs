using AutoMapper;
using Moq;
using ScrumPoker.Application.Configurations;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Services;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScrumPoker.Tests.Application.Services
{
    public class ParticipanteServiceTests
    {
        private readonly IMapper _mapper;
        public ParticipanteServiceTests()
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
            participanteBd.Online = false;

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync(participanteBd);

            var participanteInputAlteracao = new Participante();
            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Participante>()))
                    .ReturnsAsync((Participante part) => part)
                    .Callback((Participante part) => participanteInputAlteracao = part);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.Conectar(conexaoId, participanteId);

            // Assert
            Assert.Equal(participanteId, result.Id);
            Assert.Equal(conexaoId, participanteInputAlteracao.ConexaoId);
            Assert.True(result.Online);
        }

        [Fact(DisplayName = "Conectar Participante que não existe")]
        public async void ConectarParticipanteQueNaoExisteTest()
        {
            // Arrange
            string participanteId = "teste123456";
            string conexaoId = "11testete";

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

        [Fact(DisplayName = "Desconectar Participante")]
        public async void DesconectarTest()
        {
            // Arrange
            string participanteId = "teste123456";
            var participanteBd = ParticipanteValido();
            participanteBd.Id = participanteId;
            participanteBd.Online = true;

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync(participanteBd);

            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Participante>()))
                    .ReturnsAsync((Participante part) => part);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.Desconectar(participanteId);

            // Assert
            Assert.Equal(participanteId, result.Id);
            Assert.False(result.Online);
        }

        [Fact(DisplayName = "Desconectar Participante que não existe")]
        public async void DesconectarParticipanteQueNaoExisteTest()
        {
            // Arrange
            string participanteId = "teste123456";

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync((Participante)null);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.Desconectar(participanteId);

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Incluir Novo Participante")]
        public async void IncluirOuAlterarAsyncNovoParticipanteTest()
        {
            // Arrange
            var participanteInput = ParticipanteSalaInputModelValido();

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteInput.Id))
                    .ReturnsAsync((Participante)null);

            mockRepo.Setup(repo => repo.IncluirAsync(It.IsAny<Participante>()))
                    .ReturnsAsync((Participante part) => part);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.IncluirOuAlterarAsync(participanteInput);

            // Assert
            Assert.Equal(participanteInput.Id, result.Id);
            Assert.Equal(participanteInput.Jogador, result.Jogador);
            Assert.Equal(participanteInput.Nome, result.Nome);
            Assert.Equal(participanteInput.SalaId, result.SalaId);
            Assert.True(result.Online);
        }

        [Fact(DisplayName = "Alterar Participante")]
        public async void IncluirOuAlterarAsyncAlterarParticipanteTest()
        {
            // Arrange
            string participanteId = "teste123456";
            var participanteInput = ParticipanteSalaInputModelValido();
            participanteInput.Id = "teste123456";

            var participanteBd = ParticipanteValido();
            participanteBd.Id = participanteId;

            var mockRepo = new Mock<IParticipanteRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(participanteId))
                    .ReturnsAsync(participanteBd);

            var participanteInputAlteracao = new Participante();
            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Participante>()))
                    .ReturnsAsync((Participante part) => part)
                    .Callback((Participante part) => participanteInputAlteracao = part);

            var subject = new ParticipanteService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.IncluirOuAlterarAsync(participanteInput);

            // Assert
            Assert.Equal(participanteInput.Id, result.Id);
            Assert.Equal(participanteInput.Jogador, result.Jogador);
            Assert.Equal(participanteInput.Nome, result.Nome);
            Assert.Equal(participanteInput.Online, result.Online);
            Assert.Equal(participanteInput.SalaId, result.SalaId);
            Assert.Equal(participanteBd.VotoCartaValor, participanteInputAlteracao.VotoCartaValor);
            Assert.Equal(participanteBd.ConexaoId, participanteInputAlteracao.ConexaoId);
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

        private IncluirParticipanteSalaInputModel ParticipanteSalaInputModelValido()
        {
            return new IncluirParticipanteSalaInputModel()
            {
                Id = "participante123",
                SalaId = "teste123",
                //ConexaoId = "conexao123",
                Nome = "Bruno",
                Jogador = true,
                // Online = true
                //VotoCartaValor = "03"
            };
        }
    }
}