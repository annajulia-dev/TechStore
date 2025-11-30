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
            return View(produtos);
        }

        public IActionResult Enter()
        {
            return View();
        }

        public IActionResult PegarFoto(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null || produto.Foto == null)
            {
                return NotFound();
            }
            // Retorna o arquivo (bytes, tipo mime) 
            return File(produto.Foto, "image/jpeg");
        }
    }
}
