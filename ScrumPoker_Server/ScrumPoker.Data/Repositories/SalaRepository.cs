using System;
using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data.Context;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Domain.Interfaces.Repositories;
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
                                .Include(x => x.Configuracao.Cartas
                                                                    .OrderBy(x => x.Especial)
                                                                    .ThenBy(x => x.Ordem))
                                .Include(x => x.Participantes)
                                .FirstOrDefaultAsync(x => x.Id == id);

            return sala;
        }

        public async Task<bool> ExisteSalaAsync(string id)
        {
            return await _context.Set<Sala>()
                           .AnyAsync(x => x.Id == id);
        }

        public async Task LimparSalasNaoUsadas()
        {
            int partirDeQuantasSalasSeraRealizadoLimpeza = 100;
            int salasParaPermanecerSalvas = 60;

            if (await _context.Set<Sala>().CountAsync() <= partirDeQuantasSalasSeraRealizadoLimpeza)
            {
                return;
            }

            var salasParaRemover = _context.Set<Sala>()
                .OrderByDescending(x => x.UltimaDataDeUtilizacao)
                .Skip(salasParaPermanecerSalvas);

            _context.Set<Sala>().RemoveRange(salasParaRemover);
            await _context.SaveChangesAsync();
        }
    }
}