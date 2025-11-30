using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Controllers
{
    public class ServicoController : Controller
    {
        private readonly AppDbContext _context;

        public ServicoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Ao invés de _context.Servicos.ToListAsync();
            // Use o Select para escolher SÓ o que vai aparecer na tabela
            // IMPORTANTE: Não inclua a propriedade Foto aqui!

            var lista = await _context.Servicos
                .Select(s => new Servico
                {
                    Id = s.Id,
                    Titulo = s.Titulo,
                    Descricao = s.Descricao,
                    Valor = s.Valor
                    // Foto = s.Foto <--- NÃO COLOQUE ISSO
                })
                .ToListAsync();

            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Servico servico)
        {
            if (ModelState.IsValid)
            {
                // Se o usuário enviou arquivo
                if (servico.ArquivoFoto != null && servico.ArquivoFoto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await servico.ArquivoFoto.CopyToAsync(memoryStream);
                        // Validação extra: Limitar tamanho (ex: 5MB) para não travar o banco
                        if (memoryStream.Length < 5242880)
                        {
                            servico.Foto = memoryStream.ToArray();
                        }
                        else
                        {
                            ModelState.AddModelError("ArquivoFoto", "O arquivo não pode ser maior que 5MB.");
                            return View(servico);
                        }
                    }
                }

                _context.Servicos.Add(servico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(servico);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var servico = await _context.Servicos.FirstOrDefaultAsync(s => s.Id == id);

            if (servico == null)
                return NotFound();

            return View(servico);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Servico servico)
        {
            if (ModelState.IsValid)
            {
                _context.Servicos.Update(servico);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(servico);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var servico = await _context.Servicos.FindAsync(id);

            if (servico == null)
                return NotFound();

            return View(servico);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var servico = await _context.Servicos.FindAsync(id);

            if (servico != null)
            {
                _context.Servicos.Remove(servico);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult PegarFoto(int id)
        {
            var servico = _context.Servicos.Find(id);
            if (servico == null || servico.Foto == null)
            {
                return NotFound();
            }
            // Retorna o arquivo (bytes, tipo mime) 
            return File(servico.Foto, "image/jpeg");
        }
    }
}
