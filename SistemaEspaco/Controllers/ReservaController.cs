using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEspaco.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaEspaco.Controllers
{
    public class ReservaController : Controller
    {
        private readonly ProjetoEspacoContext _context;
        private readonly ReservaService _reservaService;

        public ReservaController(
            ProjetoEspacoContext context,
            ReservaService reservaService)
        {
            _context = context;
            _reservaService = reservaService;
        }

        // GET: Reserva
        public async Task<IActionResult> Index()
        {
            
            var agora = DateTime.Now;

            var reservasParaFinalizar = await _context.Reservas
                .Where(r => r.Situacao == "Ativa" && r.DataFim < agora)
                .ToListAsync();

            if (reservasParaFinalizar.Any())
            {
                foreach (var r in reservasParaFinalizar)
                {
                    r.Situacao = "Finalizada";
                }

                await _context.SaveChangesAsync();
            }

            
            ViewBag.ReservasAtivas = await _context.Reservas
                .CountAsync(r => r.Situacao == "Ativa");

            ViewBag.ReservasFinalizadas = await _context.Reservas
                .CountAsync(r => r.Situacao == "Finalizada");

            ViewBag.ReservasPorEspaco = await _context.Reservas
                .Include(r => r.IdEspacoNavigation)
                .GroupBy(r => r.IdEspacoNavigation.Nome)
                .Select(g => new
                {
                    Espaco = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            
            var reservas = _context.Reservas
                .Include(r => r.IdEspacoNavigation)
                .Include(r => r.IdUsuarioNavigation);

            return View(await reservas.ToListAsync());
        }

        // GET: Reserva/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.IdEspacoNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefaultAsync(r => r.IdReserva == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        // GET: Reserva/Create
        public IActionResult Create()
        {
            ViewData["IdEspaco"] = new SelectList(_context.Espacos, "IdEspaco", "Nome");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nome");
            return View();
        }

        // POST: Reserva/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdUsuario,IdEspaco,DataInicio,DataFim")] Reserva reserva)
        {
            if (reserva.IdEspaco == null)
            {
                ModelState.AddModelError("", "Você deve selecionar um espaço.");
            }
            else if (_reservaService.TemConflito(
                reserva.IdEspaco.Value,
                reserva.DataInicio,
                reserva.DataFim))
            {
                ModelState.AddModelError("", "Já existe uma reserva nesse horário.");
            }

            if (ModelState.IsValid)
            {
                
                reserva.Situacao = "Ativa";

                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdEspaco"] = new SelectList(
                _context.Espacos, "IdEspaco", "Nome", reserva.IdEspaco);

            ViewData["IdUsuario"] = new SelectList(
                _context.Usuarios, "IdUsuario", "Nome", reserva.IdUsuario);

            return View(reserva);
        }

        // GET: Reserva/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            ViewData["IdEspaco"] = new SelectList(
                _context.Espacos, "IdEspaco", "Nome", reserva.IdEspaco);

            ViewData["IdUsuario"] = new SelectList(
                _context.Usuarios, "IdUsuario", "Nome", reserva.IdUsuario);

            return View(reserva);
        }

        // POST: Reserva/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IdReserva,IdUsuario,IdEspaco,DataInicio,DataFim")] Reserva reserva)
        {
            if (id != reserva.IdReserva)
                return NotFound();

            var reservaBanco = await _context.Reservas
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdReserva == id);

            if (reservaBanco == null)
                return NotFound();

            if (_reservaService.TemConflito(
                reserva.IdEspaco.Value,
                reserva.DataInicio,
                reserva.DataFim,
                reserva.IdReserva))
            {
                ModelState.AddModelError("", "Já existe uma reserva nesse horário.");
            }

            if (ModelState.IsValid)
            {
               
                reserva.Situacao = reservaBanco.Situacao;

                _context.Update(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdEspaco"] = new SelectList(
                _context.Espacos, "IdEspaco", "Nome", reserva.IdEspaco);

            ViewData["IdUsuario"] = new SelectList(
                _context.Usuarios, "IdUsuario", "Nome", reserva.IdUsuario);

            return View(reserva);
        }

        // GET: Reserva/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.IdEspacoNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefaultAsync(r => r.IdReserva == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        // POST: Reserva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
