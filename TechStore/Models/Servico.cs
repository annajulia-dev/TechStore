using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Models
{
    public class Servico
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        [Required]
        public decimal Valor { get; set; }
        public byte[]? Foto { get; set; }

        // not mapped = nao vai pro bd
        [NotMapped]
        public IFormFile? ArquivoFoto { get; set; }
    }
}
