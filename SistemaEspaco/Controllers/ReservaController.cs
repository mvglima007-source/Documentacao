using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEspaco.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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

        public async Task<IActionResult> Index()
        {
            var agora = DateTime.Now;

            var reservasParaFinalizar = await _context.Reservas
                .Where(r => r.Situacao == "Ativa" && r.DataFim < agora)
                .ToListAsync();

            if (reservasParaFinalizar.Any())
            {
                foreach (var r in reservasParaFinalizar)
                    r.Situacao = "Finalizada";

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

        public IActionResult Create()
        {
            ViewData["IdEspaco"] = new SelectList(_context.Espacos, "IdEspaco", "Nome");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nome");
            return View();
        }

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

    if (reserva.DataFim <= reserva.DataInicio)
    {
        ModelState.AddModelError("", "A data final deve ser maior que a inicial.");
    }

    if (reserva.DataInicio < DateTime.Now)
    {
        ModelState.AddModelError("", "Não é possível reservar datas passadas.");
    }

    if (!ModelState.IsValid)
    {
        ViewData["IdEspaco"] = new SelectList(
            _context.Espacos, "IdEspaco", "Nome", reserva.IdEspaco);

        ViewData["IdUsuario"] = new SelectList(
            _context.Usuarios, "IdUsuario", "Nome", reserva.IdUsuario);

        return View(reserva);
    }

    

        reserva.ValorTotal = _reservaService.CalcularValorReserva(reserva);
    reserva.Situacao = "Ativa";

    _context.Reservas.Add(reserva);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
        }

        //relatorio
        public async Task<IActionResult> RelatorioGeral(DateTime? dataInicio, DateTime? dataFim)
        {
            var query = _context.Reservas
                .Include(r => r.IdEspacoNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .AsQueryable();

            if (dataInicio.HasValue)
                query = query.Where(r => r.DataInicio >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(r => r.DataFim <= dataFim.Value);

            var reservas = await query.ToListAsync();

            ViewBag.TotalUsuarios = await _context.Usuarios.CountAsync();
            ViewBag.TotalEspacos = await _context.Espacos.CountAsync();
            ViewBag.TotalReservas = reservas.Count;
            ViewBag.ReservasAtivas = reservas.Count(r => r.Situacao == "Ativa");
            ViewBag.ReservasFinalizadas = reservas.Count(r => r.Situacao == "Finalizada");
            ViewBag.FaturamentoTotal = reservas.Sum(r => r.ValorTotal);

            ViewBag.ResumoEspacos = reservas
                .GroupBy(r => r.IdEspacoNavigation.Nome)
                .Select(g => new
                {
                    Espaco = g.Key,
                    TotalReservas = g.Count(),
                    Receita = g.Sum(r => r.ValorTotal)
                })
                .ToList();

            return View(reservas);
        }

        //relatorio
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
                ModelState.AddModelError("", "Já existe uma reserva nesse horário.");

            if (ModelState.IsValid)
            {
                reserva.Situacao = reservaBanco.Situacao;
                reserva.ValorTotal = reservaBanco.ValorTotal;

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

        public IActionResult ExportarCalendario(int id)
        {
            var reserva = _context.Reservas
                .Include(r => r.IdEspacoNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefault(r => r.IdReserva == id);

            if (reserva == null)
                return NotFound();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Comprovante de Reserva")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Espaço: {reserva.IdEspacoNavigation.Nome}");
                        col.Item().Text($"Usuário: {reserva.IdUsuarioNavigation.Nome}");

                        col.Item().Text($"Data de Início: {reserva.DataInicio:dd/MM/yyyy HH:mm}");
                        col.Item().Text($"Data de Fim: {reserva.DataFim:dd/MM/yyyy HH:mm}");

                        col.Item().Text($"Situação: {reserva.Situacao}");

                        col.Item().LineHorizontal(1);

                        col.Item().Text($"Valor Total: R$ {reserva.ValorTotal:N2}")
                            .Bold();
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Sistema de Gerenciamento de Espaços")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);
                });
            });

            var bytes = pdf.GeneratePdf();

            return File(bytes, "application/pdf", "reserva.pdf");
        }
    }
}
