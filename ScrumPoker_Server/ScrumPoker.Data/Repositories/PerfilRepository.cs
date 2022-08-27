using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data.Context;
using ScrumPoker.Domain.Entities.Perfis;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
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

        public async Task<Perfil> BuscarPorIdAsync(string login)
        {
            return await _context.Set<Perfil>()
                           .FirstOrDefaultAsync(x => x.Login == login);
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

        public async Task<bool> ExisteEntityAsync(String login)
        {
            return await _context.Set<Perfil>()
                           .AnyAsync(x => x.Login == login);
        }
    }
}