using System.ComponentModel.DataAnnotations;

namespace TechStore.Models
{
    public class Produto
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        [Required]
        public decimal Valor { get; set; }
        public string? Url_foto { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
