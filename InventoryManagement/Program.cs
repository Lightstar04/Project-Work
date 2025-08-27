using InventoryManagement.Domain.Entities;
using InventoryManagement.Hubs;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Classes;
using InventoryManagement.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("logs/logs_txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
        .WriteTo.File("logs/errors_txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    );
    builder.Logging.ClearProviders();

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<InventoryManagementDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys")))
        .SetApplicationName("InventoryManagement");

    builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
    })
        .AddEntityFrameworkStores<InventoryManagementDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddSignalR();

    builder.Services.AddScoped<ICustomIdService, CustomIdService>();
    builder.Services.AddScoped<IInventoryService, InventoryService>();
    builder.Services.AddScoped<IItemService, ItemService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IHomeService, HomeService>();
    builder.Services.AddScoped<IAdminService, AdminService>();

    builder.Services.AddTransient<CustomIdService>();
    builder.Services.AddTransient<InventoryService>();
    builder.Services.AddTransient<ItemService>();
    builder.Services.AddTransient<NotificationService>();
    builder.Services.AddTransient<UserService>();
    builder.Services.AddTransient<HomeService>();
    builder.Services.AddTransient<AdminService>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var db = services.GetRequiredService<InventoryManagementDbContext>();
        db.Database.Migrate();

        var userMgr = services.GetRequiredService<UserManager<AppUser>>();
        var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedDatabase.EnsureSeedAsync(userMgr, roleMgr, db);
    }

    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JEaF5cXmRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXdcdHRUQ2NfVU10W0tWYEk=");

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

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapHub<InventoryHub>("/hubs/inventory");

    app.Run();
}
catch(Exception ex)
{
    Log.Error(ex.Message);
}
