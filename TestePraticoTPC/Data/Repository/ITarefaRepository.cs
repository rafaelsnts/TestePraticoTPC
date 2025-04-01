using TestePraticoTPC.Models;

namespace TestePraticoTPC.Data.Repository
{
    public interface ITarefaRepository
    {
        Task<Tarefas> GetByIdAsync(int id);
        Task<IEnumerable<Tarefas>> GetByUsuarioIdAsync(int usuarioId);
        Task<Tarefas> CreateAsync(Tarefas tarefa);
        Task<bool> DeleteAsync(int id);
    }
}
