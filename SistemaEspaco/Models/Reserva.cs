using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEspaco.Models
{
    public partial class Reserva
    {
        [Display(Name = "Código da Reserva")]
        public int IdReserva { get; set; }

        [Display(Name = "Usuário")]
        public int? IdUsuario { get; set; }

        [Display(Name = "Espaço")]
        public int? IdEspaco { get; set; }

        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Situação")]
        public string? Situacao { get; set; }

        public decimal ValorTotal { get; set; }


        [Display(Name = "Espaço")]
        public virtual Espaco? IdEspacoNavigation { get; set; }

        [Display(Name = "Usuário")]
        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
