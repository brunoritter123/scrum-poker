using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data.Context;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Domain.Interfaces.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ScrumPoker.Data.Repositories
{
    public class SalaConfiguracaoRepository : ISalaConfiguracaoRepository
    {
        private readonly ScrumPokerContext _context;
        public SalaConfiguracaoRepository(ScrumPokerContext context)
        {
            _context = context;
        }

        public async Task<SalaConfiguracao> IncluirAsync(SalaConfiguracao salaConfiguracao)
        {
            await _context.Set<SalaConfiguracao>().AddAsync(salaConfiguracao);
            await _context.SaveChangesAsync();

            return salaConfiguracao;
        }

        public async Task<SalaConfiguracao> AlterarAsync(SalaConfiguracao salaConfiguracao)
        {
            _context.Set<SalaConfiguracao>().Update(salaConfiguracao);
            await _context.SaveChangesAsync();

            return salaConfiguracao;
        }

        public async Task<SalaConfiguracao> BuscarPorIdAsync(long id)
        {
            var salaConfiguracao = await _context.Set<SalaConfiguracao>()
                                .Include(x => x.Cartas.OrderBy(x => x.Ordem))
                                .FirstOrDefaultAsync(x => x.Id == id);

            return salaConfiguracao;
        }

        public async Task<bool> ExisteEntityAsync(long id)
        {
            return await _context.Set<SalaConfiguracao>()
                           .AnyAsync(x => x.Id == id);
        }

        public async Task ExcluirCartasAsync(long id)
        {
            var salaConfiguracao = await BuscarPorIdAsync(id);
            _context.Set<Carta>().RemoveRange(salaConfiguracao.Cartas);
            await _context.SaveChangesAsync();
        }
    }
}