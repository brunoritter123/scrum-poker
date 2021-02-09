using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data.Context;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Repositories
{
    public class ParticipanteRepository : IParticipanteRepository
    {
        private readonly ScrumPokerContext _context;
        public ParticipanteRepository(ScrumPokerContext context)
        {
            _context = context;
        }

        public async Task<Participante> IncluirAsync(Participante salaParticipante)
        {
            await _context.Set<Participante>().AddAsync(salaParticipante);
            await _context.SaveChangesAsync();

            return salaParticipante;
        }

        public async Task<Participante> AlterarAsync(Participante salaParticipante)
        {
            _context.Set<Participante>().Update(salaParticipante);
            await _context.SaveChangesAsync();

            return salaParticipante;
        }

        public async Task<Participante> BuscarPorIdAsync(string id)
        {
            var salaParticipante = await _context.Set<Participante>()
                                .FirstOrDefaultAsync(x => x.Id == id);

            return salaParticipante;
        }

        public async Task<bool> ExisteEntityAsync(string id)
        {
            return await _context.Set<Participante>()
                           .AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Participante>> BuscarJogadoresPorSalaAsync(string salaId)
        {
            return await _context.Set<Participante>()
            .Where(x => x.SalaId == salaId && x.Jogador)
            .OrderBy(x => x.Nome)
            .ToListAsync();
        }

        public async Task<IEnumerable<Participante>> BuscarAdministradoresPorSalaAsync(string salaId)
        {
            return await _context.Set<Participante>()
            .Where(x => x.SalaId == salaId && !x.Jogador)
            .OrderBy(x => x.Nome)
            .ToListAsync();
        }

        public async Task RemoverAsync(string id)
        {
            Participante participante = await BuscarPorIdAsync(id);
            _context.Set<Participante>().Remove(participante);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Participante>> BuscarParticipantesPorSalaAsync(string salaId)
        {
            return await _context.Set<Participante>()
            .Where(x => x.SalaId == salaId)
            .OrderBy(x => x.Nome)
            .ToListAsync();
        }
    }
}