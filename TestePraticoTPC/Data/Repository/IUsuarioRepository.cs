using TestePraticoTPC.Models;

namespace TestePraticoTPC.Data.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuarios> GetByIdAsync(int id);
        Task<IEnumerable<Usuarios>> GetAllAsync();
        Task<Usuarios> GetByEmailAsync(string email);
        Task<Usuarios> CreateAsync(Usuarios usuario);
        Task<bool> UsuarioExistsAsync(int id);
    }
}
