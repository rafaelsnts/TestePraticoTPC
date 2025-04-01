using Microsoft.AspNetCore.Mvc;
using TestePraticoTPC.Data.Repository;
using TestePraticoTPC.Models;
using TestePraticoTPC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TestePraticoTPC.Controllers
{
    /// <summary>
    /// Controlador para gerenciar tarefas.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TarefasController(ITarefaRepository tarefaRepository, IUsuarioRepository usuarioRepository)
        {
            _tarefaRepository = tarefaRepository;
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Obtém uma tarefa específica pelo ID.
        /// </summary>
        /// <param name="id">ID da tarefa.</param>
        /// <returns>Retorna a tarefa encontrada ou NotFound se não existir.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaDTO>> GetTarefa(int id)
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status,
                UsuarioId = tarefa.UsuarioId
            };
        }

        /// <summary>
        /// Obtém todas as tarefas de um usuário específico.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        [HttpGet("{id}/tarefas")]
        public async Task<ActionResult<IEnumerable<TarefaDTO>>> GetTarefas(int id)
        {
            if (!await _usuarioRepository.UsuarioExistsAsync(id))
            {
                return NotFound();
            }

            var tarefas = await _tarefaRepository.GetByUsuarioIdAsync(id);
            return Ok(tarefas.Select(t => new TarefaDTO
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Status = t.Status,
                UsuarioId = t.UsuarioId
            }));
        }

        /// <summary>
        /// Cria uma nova tarefa para o usuário autenticado.
        /// </summary>
        /// <param name="tarefaDto">Dados da nova tarefa</param>
        [HttpPost("tarefas")]
        public async Task<ActionResult<TarefaDTO>> CreateTarefa(TarefaDTO tarefaDto)
        {
            // Obtendo o ID do usuário autenticado
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verificar se o usuário existe
            if (!await _usuarioRepository.UsuarioExistsAsync(usuarioId))
            {
                return NotFound();
            }

            // Criando a tarefa associada ao usuário autenticado
            var tarefa = new Tarefas
            {
                Titulo = tarefaDto.Titulo,
                Descricao = tarefaDto.Descricao,
                Status = tarefaDto.Status,
                UsuarioId = usuarioId // Usando o usuário logado
            };

            // Salva a tarefa no repositório
            await _tarefaRepository.CreateAsync(tarefa);

            // Prepara o DTO da tarefa para retornar
            tarefaDto.Id = tarefa.Id;

            // Retorna a resposta com o status Created
            return CreatedAtAction(nameof(GetTarefas), new { id = usuarioId }, tarefaDto);
        }

        /// <summary>
        /// Exclui uma tarefa pelo ID.
        /// </summary>
        /// <param name="id">ID da tarefa.</param>
        /// <returns>Retorna NoContent se excluído com sucesso ou NotFound se a tarefa não existir.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            var result = await _tarefaRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
