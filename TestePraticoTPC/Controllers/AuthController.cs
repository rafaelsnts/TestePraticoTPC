using Microsoft.AspNetCore.Mvc;
using TestePraticoTPC.Data.Repository;
using TestePraticoTPC.Models.DTOs;
using TestePraticoTPC.Services;
using System.Security.Cryptography;
using System.Text;

namespace TestePraticoTPC.Controllers
{
    /// <summary>
    /// Controlador responsável pela autenticação de usuários.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;

        /// <summary>
        /// Construtor do AuthController.
        /// </summary>
        /// <param name="usuarioRepository">Repositório de usuários.</param>
        /// <param name="tokenService">Serviço de geração de token.</param>
        public AuthController(IUsuarioRepository usuarioRepository, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Autentica um usuário e retorna um token JWT.
        /// </summary>
        /// <param name="loginDto">Dados do usuário (email e senha).</param>
        /// <returns>Token JWT em caso de sucesso.</returns>
        /// <response code="200">Retorna o token JWT.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO loginDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);

            if (usuario == null)
            {
                return Unauthorized("Email ou senha inválidos");
            }

            var senhaHash = HashPassword(loginDto.Senha);
            if (usuario.Senha != senhaHash)
            {
                return Unauthorized("Email ou senha inválidos");
            }

            var token = _tokenService.GerarToken(usuario);

            return Ok(new { token });
        }

        /// <summary>
        /// Gera um hash SHA256 da senha informada.
        /// </summary>
        /// <param name="password">Senha em texto plano.</param>
        /// <returns>Senha criptografada em Base64.</returns>
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
