namespace ScrumPoker.Domain.Models
{
    public class Carta
    {
        public int Id { get; set; }
        public int Ordem { get; set; }
        public string Value { get; set; }
        public bool Especial { get; set;}


        public string SalaId { get; set; }
        public Sala Sala { get; }
    }
}