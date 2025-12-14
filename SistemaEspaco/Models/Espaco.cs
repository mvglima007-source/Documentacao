using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEspaco.Models;

public partial class Espaco
{
    public int IdEspaco { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }
    [Range(1, 45, ErrorMessage = "A capacidade deve estar entre {1} e {2}.")]
    public int? Capacidade { get; set; }

    [Required(ErrorMessage = "Informe o valor por hora")]
    [Range(1, 9999)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorHora { get; set; }



    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
