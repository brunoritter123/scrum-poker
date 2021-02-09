namespace ScrumPoker.Tests.Core.Domain
{
    public class ParticipanteTests
    {

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