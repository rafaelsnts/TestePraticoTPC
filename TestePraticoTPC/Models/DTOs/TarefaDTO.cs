using System.ComponentModel.DataAnnotations;

namespace TestePraticoTPC.Models.DTOs
{
    public class TarefaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título da tarefa deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição da tarefa é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição da tarefa deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Range(0, 2, ErrorMessage = "O status da tarefa deve ser entre 0 (Pendente), 1 (Em Andamento) ou 2 (Concluído).")]
        public int Status { get; set; }

        public string StatusDescricao => Status switch
        {
            0 => "Pendente",
            1 => "Em Andamento",
            2 => "Concluído",
            _ => "Desconhecido"
        };

        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        public int UsuarioId { get; set; }
    }
}
