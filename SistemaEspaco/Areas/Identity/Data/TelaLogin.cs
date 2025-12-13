using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SistemaEspaco.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TelaLogin class
public class TelaLogin : IdentityUser
{
    public string Nome { get; set; }
}

