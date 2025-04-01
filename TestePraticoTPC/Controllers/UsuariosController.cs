using Microsoft.AspNetCore.Mvc;
using TestePraticoTPC.Data.Repository;
using TestePraticoTPC.Models;
using TestePraticoTPC.Models.DTOs;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace TestePraticoTPC.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão de usuários.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITarefaRepository _tarefaRepository;

        public UsuariosController(IUsuarioRepository usuarioRepository, ITarefaRepository tarefaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tarefaRepository = tarefaRepository;
        }

        /// <summary>
        /// Obtém todos os usuários cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return Ok(usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email
            }));
        }

        /// <summary>
        /// Obtém um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="registerDto">Dados do usuário a ser cadastrado</param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> CreateUsuario(RegisterDTO registerDto)
        {
            if (await _usuarioRepository.GetByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("Email já cadastrado");
            }

            var usuario = new Usuarios
            {
                Nome = registerDto.Nome,
                Email = registerDto.Email,
                Senha = HashPassword(registerDto.Senha)
            };

            await _usuarioRepository.CreateAsync(usuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
