using Microsoft.EntityFrameworkCore;
using RfidAccess.Web.DataAccess.Context;
using RfidAccess.Web.DataAccess.Repositories.People;
using RfidAccess.Web.DataAccess.Repositories.Records;
using RfidAccess.Web.Services.Buffer;
using RfidAccess.Web.Services.Records;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? dataConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RfidContext>(options => 
    options.UseSqlite(dataConnection));

builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<IRecordRepository, RecordRepository>();

builder.Services.AddScoped<IRecordService, RecordService>();

builder.Services.AddSingleton(new PersonBufferService());

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetService<RfidContext>();
    context?.Database.Migrate();
}

app.Run();
