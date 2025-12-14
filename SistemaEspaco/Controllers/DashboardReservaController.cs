using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEspaco.Models;
using SistemaEspaco.Models.ViewModels;
using System.Threading.Tasks;

namespace SistemaEspaco.Controllers
{
    public class DashboardReservaController : Controller
    {
        private readonly ProjetoEspacoContext _context;

        public DashboardReservaController(ProjetoEspacoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var total = await _context.Reservas.CountAsync();
            var ativas = await _context.Reservas
                .CountAsync(r => r.Situacao == "Ativa");
            var finalizadas = await _context.Reservas
                .CountAsync(r => r.Situacao == "Finalizada");

            var vm = new DashboardReserva
            {
                TotalReservas = total,
                ReservasAtivas = ativas,
                ReservasFinalizadas = finalizadas
            };

            return View(vm);


        }


    }

}
