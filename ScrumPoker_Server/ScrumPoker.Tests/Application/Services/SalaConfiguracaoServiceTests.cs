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
    public class SalaConfiguracaoTests
    {
        private readonly IMapper _mapper;
        public SalaConfiguracaoTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperApiConfig());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact(DisplayName = "Buscar por ID")]
        public async void BuscarPorIdAsyncTest()
        {
            // Arrange
            long id = 120;
            var salaConfig = SalaValida();
            salaConfig.Id = id;

            var mockRepo = new Mock<ISalaConfiguracaoRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync(salaConfig);

            var subject = new SalaConfiguracaoService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.BuscarPorIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SalaConfiguracaoViewModel>(result);
            Assert.Equal(id, result.Id);
        }

        [Fact(DisplayName = "Buscar por ID que não existe")]
        public async void BuscarPorIdAsync_OndeIdNaoExisteTest()
        {
            // Arrange
            long id = 120;

            var mockRepo = new Mock<ISalaConfiguracaoRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(id))
                    .ReturnsAsync((SalaConfiguracao)null);

            var subject = new SalaConfiguracaoService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.BuscarPorIdAsync(id);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(id.ToString(), exception.Message);
        }

        [Fact(DisplayName = "Alterar SalaConfiguracao")]
        public async void AlterarAsyncTest()
        {
            // Arrange
            var inputModel = AlterarConfiguracaoSalaInputModelValida();
            inputModel.JogadorFinalizaJogo = true;
            inputModel.JogadorRemoveAdministrador = true;
            inputModel.JogadorResetaJogo = false;

            var salaConfigBd = SalaValida();
            SalaConfiguracao salaConfigReturnoAlteracao =  new SalaConfiguracao();

            var mockRepo = new Mock<ISalaConfiguracaoRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(inputModel.Id))
                    .ReturnsAsync(salaConfigBd);
            mockRepo.Setup(repo => repo.AlterarAsync(It.IsAny<SalaConfiguracao>()))
                    .Callback<SalaConfiguracao>(x => salaConfigReturnoAlteracao = x)
                    .ReturnsAsync(salaConfigReturnoAlteracao);

            var subject = new SalaConfiguracaoService(mockRepo.Object, _mapper);

            // Act
            var result = await subject.AlterarAsync(inputModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SalaConfiguracaoViewModel>(result);
            Assert.Equal(inputModel.Id, result.Id);
            Assert.Equal(inputModel.JogadorFinalizaJogo, result.JogadorFinalizaJogo);
            Assert.Equal(inputModel.JogadorRemoveAdministrador, result.JogadorResetaJogo);
        }

        [Fact(DisplayName = "Alterar de uma SalaConfiguracao que não existe")]
        public async void AlterarAsync_OndeIdNaoExisteTest()
        {
            // Arrange
            var inputModel = AlterarConfiguracaoSalaInputModelValida();

            var mockRepo = new Mock<ISalaConfiguracaoRepository>();
            mockRepo.Setup(repo => repo.BuscarPorIdAsync(inputModel.Id))
                    .ReturnsAsync((SalaConfiguracao)null);

            var subject = new SalaConfiguracaoService(mockRepo.Object, _mapper);

            // Act
            Func<Task> act = () => subject.AlterarAsync(inputModel);

            // Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(act);
            Assert.Contains(inputModel.Id.ToString(), exception.Message);
        }

        private SalaConfiguracao SalaValida()
        {
            return new SalaConfiguracao()
            {
                Id = 123456,
                SalaId = "Sala Simples",
                JogadorFinalizaJogo = false,
                JogadorRemoveJogador = false,
                JogadorRemoveAdministrador = false,
                JogadorResetaJogo = false,
                Cartas = new List<Carta>()
                {

                }
            };
        }

        private AlterarConfiguracaoSalaInputModel AlterarConfiguracaoSalaInputModelValida()
        {
            var cartasInput = new List<AlterarConfiguracaoSalaCartaInputModel>()
            {
                new AlterarConfiguracaoSalaCartaInputModel()
                {
                    Id = 1,
                    Ordem = 1,
                    Value = "1",
                    Especial = false,
                    SalaId = "SalaID"
                }
            };

            AlterarConfiguracaoSalaInputModel inputModel = new AlterarConfiguracaoSalaInputModel()
            {
                Id = 120,
                SalaId = "SalaID",
                JogadorFinalizaJogo = false,
                JogadorRemoveAdministrador = false,
                JogadorRemoveJogador = false,
                JogadorResetaJogo = false,
                Cartas = cartasInput
            };

            return inputModel;
        }
    }
}
