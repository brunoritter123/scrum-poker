namespace ScrumPoker.Domain.Entities.SalaEntity
{
    public class Participante
    {
        public string Id { get; set; }
        public string SalaId { get; set; }
        public string ConexaoId { get; set; }
        public string Nome { get; set; }
        public bool Jogador { get; set; }
        public bool Online { get; set; }

        public string VotoCartaValor { get; set; }

        public Sala Sala { get; }
    }
}