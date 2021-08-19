using Xunit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Application.Services;
using AutoMapper;
using ScrumPoker.Application.Configurations;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.DTOs.InputModels;

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

        private Sala SalaValida(string idSala)
        {
            var sala = new Sala()
            {
                Id = idSala,
                JogoFinalizado = false,
                UltimaDataDeUtilizacao = DateTime.Now,
                Titulo = "Titulo"
            };

            sala.Configuracao = new SalaConfiguracao()
            {
                SalaId = idSala,
                JogadorFinalizaJogo = false,
                JogadorResetaJogo = false,
                JogadorRemoveJogador = false,
                JogadorRemoveAdministrador = true,
            };

            return sala;
        }
    }
}
