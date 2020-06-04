using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data.Context;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Data.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        private readonly ScrumPokerContext _context;
        public PerfilRepository(ScrumPokerContext context)
        {
            _context = context;
        }

        public async Task<Perfil> BuscarPorIdAsync(Guid id)
        {
            return await _context.Set<Perfil>()
                           .Include(x => x.User)
                           .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Perfil> IncluirAsync(Perfil perfil)
        {
            await _context.Set<Perfil>().AddAsync(perfil);
            await _context.SaveChangesAsync();

            return perfil;
        }

        public async Task<Perfil> AlterarAsync(Perfil perfil)
        {
            _context.Set<Perfil>().Update(perfil);
            await _context.SaveChangesAsync();

            return perfil;
        }

        public async Task<bool> ExisteEntityAsync(Guid id)
        {
            return await _context.Set<Perfil>()
                           .AnyAsync(x => x.Id == id);
        }
    }
}