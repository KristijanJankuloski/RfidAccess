using Microsoft.EntityFrameworkCore;
using RfidAccess.Web.DataAccess.Context;
using RfidAccess.Web.DataAccess.Repositories.People;
using RfidAccess.Web.DataAccess.Repositories.Records;
using RfidAccess.Web.DataAccess.Repositories.TimeSlots;
using RfidAccess.Web.Services.Buffer;
using RfidAccess.Web.Services.Export;
using RfidAccess.Web.Services.People;
using RfidAccess.Web.Services.Records;
using RfidAccess.Web.Services.Schedules;
using RfidAccess.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RfidAccess.Web.Hub;
using RfidAccess.Web.Services.HubService;
using RfidAccess.Web.DataAccess.Repositories.ErrorLogs;
using RfidAccess.Web.Services.ErrorLogs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? dataConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RfidContext>(options => 
    options.UseSqlite(dataConnection));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<RfidContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccesssDenied";
});

builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<IRecordRepository, RecordRepository>();
builder.Services.AddTransient<IWeekTimeSlotsRepository, WeekTimeSlotsRepository>();
builder.Services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
builder.Services.AddTransient<IHubService, HubService>();

builder.Services.AddScoped<IRecordService, RecordService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();

builder.Services.AddSingleton(new PersonBufferService());

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<NotificationHub>("/notifications");

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetService<RfidContext>();
    context?.Database.Migrate();
}

app.Run();
