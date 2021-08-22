using AutoMapper;
using Moq;
using ScrumPoker.Application.Configurations;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Services;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Domain.Entities.UsuarioEntity;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScrumPoker.Tests.Application.Services
{
    public class PerfilServiceTests
    {
        private readonly IMapper _mapper;
        public PerfilServiceTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperApiConfig());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact(DisplayName = "Perfil Participante Por Id")]
        public async void BuscarParticipantePorIdTest()
        {
            // Arrange
            Guid perfilId = Guid.NewGuid();
            var perfilBd = PerfilValido();
            perfilBd.Id = perfilId;

            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(perfilId))
                    .ReturnsAsync(perfilBd);

            var subject = new PerfilService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.BuscarPorIdAsync(perfilId);

            // Assert
            mockRepo.Verify(mock => mock.BuscarPorIdAsync(perfilId), Times.Once());
            Assert.IsType<PerfilViewModel>(result);
            Assert.Equal(perfilId, result.Id);
        }

        [Fact(DisplayName = "Buscar Perfil Por Id Que Não Existe")]
        public async void BuscarParticipantePorIdQueNaoExisteTest()
        {
            // Arrange
            Guid perfilId = Guid.NewGuid();

            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(perfilId))
                    .ReturnsAsync((Perfil)null);

            var subject = new PerfilService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.BuscarPorIdAsync(perfilId);

            // Assert
            mockRepo.Verify(mock => mock.BuscarPorIdAsync(perfilId), Times.Once());
            Assert.Null(result);
        }

        [Fact(DisplayName = "Alterar Perfil")]
        public async void AlterarAsyncTest()
        {
            // Arrange
            Guid perfilId = Guid.NewGuid();

            var inputModel = new PerfilAlteracaoInputModel();
            inputModel.Id = perfilId;
            inputModel.Nome = "Nome Alterado";
            inputModel.Email = "emailAlterado@teste.com";

            var perfilBd = PerfilValido();
            perfilBd.Id = perfilId;
            perfilBd.Nome = "Nome Original";

            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(perfilId))
                    .ReturnsAsync(perfilBd);

            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<Perfil>()))
                    .ReturnsAsync((Perfil perfilReturno) => perfilReturno);

            var subject = new PerfilService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.AlterarAsync(inputModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(inputModel.Id, result.Id);
            Assert.Equal(inputModel.Nome, result.Nome);
        }

        [Fact(DisplayName = "Alterar Perfil que não existe")]
        public async void AlterarAsync_OndeIdNaoExisteTest()
        {
            // Arrange
            Guid perfilId = Guid.NewGuid();
            var inputModel = new PerfilAlteracaoInputModel();
            inputModel.Id = perfilId;
            inputModel.Nome = "Nome Alterado";

            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(perfilId))
                    .ReturnsAsync((Perfil)null);

            var subject = new PerfilService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.AlterarAsync(inputModel);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(inputModel.Id.ToString(), exception.Message);
        }

        private Perfil PerfilValido()
        {
            return new Perfil()
            {
                Id = Guid.NewGuid(),
                Nome = "Nome Teste"
            };
        }
    }
}