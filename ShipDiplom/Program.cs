using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShipDiplom.Database;
using ShipDiplom.Models;

namespace ShipDiplom;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables(prefix: "ENV_")
            .AddUserSecrets("AF72ABA5-6526-46CC-AFD6-CAB7550E7BC1");

        builder.Services.AddSingleton(new MapperConfiguration(mc =>
        {
            mc.AddProfile<ControllersMappingProfile>();
        }).CreateMapper());

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration["App:DbConnectionString"]);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });

        builder.Services.AddControllersWithViews();

        builder.Services.AddDomain();

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
    }
}