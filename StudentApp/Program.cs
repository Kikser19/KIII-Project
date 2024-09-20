using Microsoft.EntityFrameworkCore;
using StudentApp.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StudentAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentAppContext"), sql=>sql.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
