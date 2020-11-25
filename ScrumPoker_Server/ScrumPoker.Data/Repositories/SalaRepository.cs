using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data.Context;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Repositories
{
    public class SalaRepository : ISalaRepository
    {
        private readonly ScrumPokerContext _context;
        public SalaRepository(ScrumPokerContext context)
        {
            _context = context;
        }

        public async Task<Sala> IncluirAsync(Sala sala)
        {
            await _context.Set<Sala>().AddAsync(sala);
            await _context.SaveChangesAsync();

            return sala;
        }

        public async Task<Sala> AlterarAsync(Sala sala)
        {
            _context.Set<Sala>().Update(sala);
            await _context.SaveChangesAsync();

            return sala;
        }

        public async Task<Sala> BuscarPorIdAsync(string id)
        {
            var sala = await _context.Set<Sala>()
                                .Include(x => x.Configuracao)
                                .Include(x => x.Configuracao.Cartas)
                                .Include(x => x.Participantes)
                                .FirstOrDefaultAsync(x => x.Id == id);

            return sala;
        }

        public async Task<bool> ExisteEntityAsync(string id)
        {
            return await _context.Set<Sala>()
                           .AnyAsync(x => x.Id == id);
        }

        public async Task ExcluirCartasAsync(string id)
        {
            var sala = await BuscarPorIdAsync(id);
            _context.Set<Carta>().RemoveRange(sala.Configuracao.Cartas);
            await _context.SaveChangesAsync();
        }
    }
}