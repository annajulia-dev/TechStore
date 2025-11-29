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
            var servicos = await _context.Servicos.ToListAsync();

            return View(servicos);
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
                _context.Servicos.Add(servico);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
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
    }
}
