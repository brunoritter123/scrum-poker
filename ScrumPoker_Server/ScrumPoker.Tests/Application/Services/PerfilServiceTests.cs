using AutoMapper;
using Moq;
using ScrumPoker.Application.Configurations;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Services;
using ScrumPoker.Domain.Entities.Perfis;
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
            string perfilId = "login";
            var perfilBd = PerfilValido();
            perfilBd.Login = perfilId;

            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(perfilId))
                    .ReturnsAsync(perfilBd);

            var subject = new PerfilService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.BuscarPorLoginAsync(perfilId);

            // Assert
            mockRepo.Verify(mock => mock.BuscarPorIdAsync(perfilId), Times.Once());
            Assert.IsType<PerfilViewModel>(result);
            Assert.Equal(perfilId, result.Login);
        }

        [Fact(DisplayName = "Buscar Perfil Por Id Que Não Existe")]
        public async void BuscarParticipantePorIdQueNaoExisteTest()
        {
            // Arrange
            string perfilId = "login";

            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(perfilId))
                    .ReturnsAsync((Perfil)null);

            var subject = new PerfilService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.BuscarPorLoginAsync(perfilId);

            // Assert
            mockRepo.Verify(mock => mock.BuscarPorIdAsync(perfilId), Times.Once());
            Assert.Null(result);
        }

        [Fact(DisplayName = "Alterar Perfil")]
        public async void AlterarAsyncTest()
        {
            // Arrange
            string perfilId = "login";

            var inputModel = new PerfilAlteracaoInputModel();
            inputModel.Login = perfilId;
            inputModel.Nome = "Nome Alterado";
            inputModel.Email = "emailAlterado@teste.com";

            var perfilBd = PerfilValido();
            perfilBd.Login = perfilId;
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
            Assert.Equal(inputModel.Login, result.Login);
            Assert.Equal(inputModel.Nome, result.Nome);
        }

        [Fact(DisplayName = "Alterar Perfil que não existe")]
        public async void AlterarAsync_OndeIdNaoExisteTest()
        {
            // Arrange
            string perfilId = "login";
            var inputModel = new PerfilAlteracaoInputModel();
            inputModel.Login = perfilId;
            inputModel.Nome = "Nome Alterado";

            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(perfilId))
                    .ReturnsAsync((Perfil)null);

            var subject = new PerfilService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.AlterarAsync(inputModel);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(inputModel.Login.ToString(), exception.Message);
        }

        private Perfil PerfilValido()
        {
            return new Perfil()
            {
                Login = "login",
                Nome = "Nome Teste"
            };
        }
    }
}