using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEspaco.Models;

namespace SistemaEspaco.Controllers
{
    public class EspacoController : Controller
    {
        private readonly ProjetoEspacoContext _context;

        public EspacoController(ProjetoEspacoContext context)
        {
            _context = context;
        }

        // GET: Espaco
        public async Task<IActionResult> Index()
        {
            return View(await _context.Espacos.ToListAsync());
        }

        // GET: Espaco/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espaco = await _context.Espacos
                .FirstOrDefaultAsync(m => m.IdEspaco == id);
            if (espaco == null)
            {
                return NotFound();
            }

            return View(espaco);
        }

        // GET: Espaco/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Espaco/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEspaco,Nome,Descricao,Capacidade")] Espaco espaco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(espaco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(espaco);
        }

        // GET: Espaco/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espaco = await _context.Espacos.FindAsync(id);
            if (espaco == null)
            {
                return NotFound();
            }
            return View(espaco);
        }

        // POST: Espaco/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEspaco,Nome,Descricao,Capacidade")] Espaco espaco)
        {
            if (id != espaco.IdEspaco)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(espaco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspacoExists(espaco.IdEspaco))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(espaco);
        }

        // GET: Espaco/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espaco = await _context.Espacos
                .FirstOrDefaultAsync(m => m.IdEspaco == id);
            if (espaco == null)
            {
                return NotFound();
            }

            return View(espaco);
        }

        // POST: Espaco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var espaco = await _context.Espacos.FindAsync(id);
            if (espaco != null)
            {
                _context.Espacos.Remove(espaco);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspacoExists(int id)
        {
            return _context.Espacos.Any(e => e.IdEspaco == id);
        }
    }
}
