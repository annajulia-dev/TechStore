namespace TechStore.Models
{
    public class ProdutosViewModel
    {
        public IEnumerable<Produto> Produtos { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; }
    }
}
