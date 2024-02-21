using ASP_DZ1.Data;
using ASP_DZ1.Middleware;
using ASP_DZ1.Services.Hash;

using ASP_DZ1.Services.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;




var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("dbsettings.json");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IValidationService, MyValidationService>();


builder.Services.AddSingleton<IHashService, Md5HashService>();

String? connectionString =
    builder
    .Configuration
    .GetConnectionString("mystring");




builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlServerOptions => sqlServerOptions.MigrationsHistoryTable(
                tableName: HistoryRepository.DefaultTableName,
        schema: "ASP_DZ1")
    )
);




builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseMiddleware<AuthSessionMiddleware>();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
