using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data.Context;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Repositories
{
    public class SalaParticipanteRepository : ISalaParticipanteRepository
    {
        private readonly ScrumPokerContext _context;
        public SalaParticipanteRepository(ScrumPokerContext context)
        {
            _context = context;
        }

        public async Task<SalaParticipante> IncluirAsync(SalaParticipante salaParticipante)
        {
            await _context.Set<SalaParticipante>().AddAsync(salaParticipante);
            await _context.SaveChangesAsync();

            return salaParticipante;
        }

        public async Task<SalaParticipante> AlterarAsync(SalaParticipante salaParticipante)
        {
            _context.Set<SalaParticipante>().Update(salaParticipante);
            await _context.SaveChangesAsync();

            return salaParticipante;
        }

        public async Task<SalaParticipante> BuscarPorIdAsync(string id)
        {
            var salaParticipante = await _context.Set<SalaParticipante>()
                                .FirstOrDefaultAsync(x => x.Id == id);

            return salaParticipante;
        }

        public async Task<bool> ExisteEntityAsync(string id)
        {
            return await _context.Set<SalaParticipante>()
                           .AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<SalaParticipante>> BuscarJogadoresPorSalaAsync(string salaId)
        {
            return await _context.Set<SalaParticipante>()
            .Where(x => x.SalaId == salaId && x.Jogador)
            .OrderBy(x => x.Nome)
            .ToListAsync();
        }

        public async Task<IEnumerable<SalaParticipante>> BuscarAdministradoresPorSalaAsync(string salaId)
        {
            return await _context.Set<SalaParticipante>()
            .Where(x => x.SalaId == salaId && !x.Jogador)
            .OrderBy(x => x.Nome)
            .ToListAsync();
        }

        public async Task RemoverAsync(string id)
        {
            SalaParticipante participante = await BuscarPorIdAsync(id);
            _context.Set<SalaParticipante>().Remove(participante);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SalaParticipante>> BuscarParticipantesPorSalaAsync(string salaId)
        {
            return await _context.Set<SalaParticipante>()
            .Where(x => x.SalaId == salaId)
            .OrderBy(x => x.Nome)
            .ToListAsync();
        }
    }
}