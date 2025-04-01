using Microsoft.EntityFrameworkCore;
using TestePraticoTPC.Models;

namespace TestePraticoTPC.Data.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TestePraticoTPCContext _context;

        public UsuarioRepository(TestePraticoTPCContext context)
        {
            _context = context;
        }

        public async Task<Usuarios> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<IEnumerable<Usuarios>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuarios> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuarios> CreateAsync(Usuarios usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> UsuarioExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.Id == id);
        }
    }
}
