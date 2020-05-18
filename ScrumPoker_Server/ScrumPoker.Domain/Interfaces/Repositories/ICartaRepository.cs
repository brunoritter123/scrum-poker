using System.ComponentModel;
using System;
using ScrumPoker.Domain.Models;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface ICartaRepository
    {
        Carta BuscarPorId(int id);
        void Excluir(int id);
        bool ExisteEntity(int id);
    }
}