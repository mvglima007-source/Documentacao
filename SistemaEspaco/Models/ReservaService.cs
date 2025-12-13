using System;
using System.Linq;

namespace SistemaEspaco.Models
{
    public class ReservaService
    {
        private readonly ProjetoEspacoContext _context;

        public ReservaService(ProjetoEspacoContext context)
        {
            _context = context;
        }

        public bool TemConflito(int? idEspaco, DateTime dataInicio, DateTime dataFim, int? idReservaIgnorar = null)
        {
            return _context.Reservas.Any(r =>
                r.IdEspaco == idEspaco &&
                (idReservaIgnorar == null || r.IdReserva != idReservaIgnorar.Value) &&
                r.DataInicio < dataFim &&
                r.DataFim > dataInicio
            );
        }


    }
}
