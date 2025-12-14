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
        public decimal CalcularValorReserva(Reserva reserva)
        {
            var horas = (decimal)(reserva.DataFim - reserva.DataInicio).TotalHours;

            if (horas <= 0)
                throw new Exception("Período de reserva inválido.");

            const decimal valorHoraPadrao = 50;
            const decimal valorMaximoDiario = 200;

            var valorHora = _context.Espacos
                .Where(e => e.IdEspaco == reserva.IdEspaco)
                .Select(e => e.ValorHora)
                .FirstOrDefault();

            if (valorHora <= 0)
                valorHora = valorHoraPadrao;

            var valorCalculado = horas * valorHora;

            var dias = Math.Ceiling(horas / 24);
            var valorComTeto = dias * valorMaximoDiario;

            return Math.Min(valorCalculado, valorComTeto);
        }





    }
}
