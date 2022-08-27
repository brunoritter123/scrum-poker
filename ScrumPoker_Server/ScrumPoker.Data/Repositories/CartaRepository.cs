using ScrumPoker.Data.Context;
using ScrumPoker.Domain.Entities.Salas.Cartas;
using ScrumPoker.Domain.Interfaces.Repositories;
using System.Linq;

namespace ScrumPoker.Data.Repositories
{
    public class CartaRepository : ICartaRepository
    {
        private readonly ScrumPokerContext _context;
        public CartaRepository(ScrumPokerContext context)
        {
            _context = context;
        }

        public void Excluir(int id)
        {
            var carta = BuscarPorId(id);
            _context.Set<Carta>().Remove(carta);
        }

        public Carta BuscarPorId(int id)
        {
            return _context.Set<Carta>()
                           .FirstOrDefault(x => x.Id == id);
        }

        public bool ExisteEntity(int id)
        {
            return _context.Set<Carta>()
                           .Any(x => x.Id == id);
        }
    }
}