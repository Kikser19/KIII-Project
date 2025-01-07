using Microsoft.EntityFrameworkCore;
using StudentApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add configuration to include environment variables (this automatically loads Kubernetes ConfigMap and Secrets into the app)
builder.Configuration.AddEnvironmentVariables();

// Access the environment variables from ConfigMap (loaded into the configuration)
var appEnv = builder.Configuration["APP_ENV"];  // "production"
var appDebug = builder.Configuration["APP_DEBUG"];  // "false"

// Configure DbContext with SQL Server connection string from environment variables or appsettings
builder.Services.AddDbContext<StudentAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentAppContext"),
        sql => sql.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Additional configurations can be added here based on the loaded environment variables
if (appDebug == "true")
{
    builder.Services.AddLogging(builder => builder.AddConsole());
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
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
