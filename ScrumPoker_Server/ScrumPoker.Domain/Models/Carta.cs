namespace ScrumPoker.Domain.Models
{
    public class Carta
    {
        public int Id { get; set; }
        public int Ordem { get; set; }
        public string Value { get; set; }
        public bool Especial { get; set;}


        public long SalaConfiguracaoId { get; set; }
        public SalaConfiguracao SalaConfiguracao { get; }
    }
}