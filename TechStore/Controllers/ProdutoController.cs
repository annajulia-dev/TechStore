using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var produtos = await _context.Produtos.
                                          Include(p => p.Categoria).
                                          ToListAsync();
            return View(produtos);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }

        [HttpGet]
        public IActionResult Create()
        {

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Titulo");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Produto novoProduto)
        {
            if (ModelState.IsValid)
            {
                // Se o usuário enviou arquivo
                if (novoProduto.ArquivoFoto != null && novoProduto.ArquivoFoto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await novoProduto.ArquivoFoto.CopyToAsync(memoryStream);
                        // Validação extra: Limitar tamanho (ex: 5MB) para não travar o banco
                        if (memoryStream.Length < 5242880)
                        {
                            novoProduto.Foto = memoryStream.ToArray();
                        }
                        else
                        {
                            ModelState.AddModelError("ArquivoFoto", "O arquivo não pode ser maior que 5MB.");
                            return View(novoProduto);
                        }
                    }
                }

                _context.Produtos.Add(novoProduto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Titulo");
            return View(novoProduto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound();

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Titulo", produto.CategoriaId);

            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Produto produto, IFormFile? novaImagem)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var produtoOriginal = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);


                    if (novaImagem != null && novaImagem.Length > 0)
                    {
     
                        using (var memoryStream = new MemoryStream())
                        {
                            await novaImagem.CopyToAsync(memoryStream);
                            produto.Foto = memoryStream.ToArray(); 
                        }
                    }
                    else
                    {
                        if (produtoOriginal != null)
                        {
                            produto.Foto = produtoOriginal.Foto;
                        }
                    }

                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Produtos.Any(e => e.Id == produto.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var produtoParaDeletar = await _context.Produtos.FindAsync(id);

            if (produtoParaDeletar != null)
            {
                _context.Produtos.Remove(produtoParaDeletar);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
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
