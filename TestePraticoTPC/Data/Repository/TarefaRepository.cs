using Microsoft.EntityFrameworkCore;
using TestePraticoTPC.Models;

namespace TestePraticoTPC.Data.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly TestePraticoTPCContext _context;

        public TarefaRepository(TestePraticoTPCContext context)
        {
            _context = context;
        }

        public async Task<Tarefas> GetByIdAsync(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        public async Task<IEnumerable<Tarefas>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Tarefas
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Tarefas> CreateAsync(Tarefas tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            return tarefa;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
                return false;

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
