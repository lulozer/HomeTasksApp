using HomeTasksApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UygulamaDbContext>();

    if (!db.Users.Any(u => u.Name == "Ekin"))
    {
        var household = db.Households.FirstOrDefault(h => h.Name == "ÖzerFamily");
        db.Users.Add(new User
        {
            Name = "Ekin",
            Password = "ekin123",
            IsAdmin = false,
            Household = household!,
            IsHouseholdAdmin = false
        });
        db.SaveChanges();
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UygulamaDbContext>();

    
    var evimiz = db.Households.FirstOrDefault(h => h.Name == "Evimiz");
    if (evimiz == null)
    {
        evimiz = new Household { Name = "Evimiz" };
        db.Households.Add(evimiz);
        db.SaveChanges();
    }

    
    if (!db.Users.Any(u => u.Name == "Behiye"))
    {
        db.Users.Add(new User
        {
            Name = "Behiye",
            Password = "behiye123",
            IsAdmin = false,
            Household = evimiz,
            IsHouseholdAdmin = true
        });
    }

    
    if (!db.Users.Any(u => u.Name == "Nisa"))
    {
        db.Users.Add(new User
        {
            Name = "Nisa",
            Password = "nisa123",
            IsAdmin = false,
            Household = evimiz,
            IsHouseholdAdmin = false
        });
    }

    db.SaveChanges();
}

app.Run();  // en sonda olmalı !