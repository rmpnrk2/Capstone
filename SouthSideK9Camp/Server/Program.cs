using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Server.JSONSettings;
using SouthSideK9Camp.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// database service
var connectionString = builder.Configuration.GetConnectionString("Localhost");
builder.Services.AddSqlServer<DataContext>(connectionString);
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

// email Service
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

// apply migrations

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();