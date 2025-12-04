using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Produtos()
        {
            var produtos = await _context.Produtos.
                                          Include(p => p.Categoria).
                                          ToListAsync();

            var categorias = await _context.Categorias.ToListAsync();

            var viewModel = new ProdutosViewModel
            {
                Produtos = produtos,
                Categorias = categorias
            };

            return View(viewModel);
        }

        public IActionResult Enter()
        {
            return View();
        }

        public async Task<IActionResult> Servicos()
        {
            var servicos = await _context.Servicos.ToListAsync();
            return View(servicos);
        }
    }
}
