using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApp.Data;
using WebApp.Services;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //  Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options => {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
        });
        
        builder.Services.Configure<CookiePolicyOptions>(
         options => {
             options.CheckConsentNeeded = context => false;
             options.MinimumSameSitePolicy = SameSiteMode.None;
         });

        builder.Services.AddScoped<SessionService>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddLocalization();
        builder.Services.AddMvc()
            .AddDataAnnotationsLocalization()
            .AddViewLocalization(options => options.ResourcesPath = "Resources");

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        //app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.UseSession();

        var supportedCultures = new[] { new CultureInfo("pl-PL"), new CultureInfo("pl"), new CultureInfo("en-US"), new CultureInfo("en") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pl"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.Run();
    }
}