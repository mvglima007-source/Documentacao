using Microsoft.EntityFrameworkCore;
using SistemaEspaco.Models;
using Microsoft.AspNetCore.Identity;
using SistemaEspaco.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();



builder.Services.AddDbContext<ProjetoEspacoContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("connection")));



builder.Services.AddDbContext<SistemaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SistemaContextConnection")));

builder.Services.AddDefaultIdentity<TelaLogin>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})


.AddEntityFrameworkStores<SistemaContext>();


builder.Services.AddScoped<ReservaService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Identity precisa disso:
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

app.MapRazorPages();

app.Run();
