using Microsoft.Extensions.DependencyInjection;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Extensions;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using AutoMapper;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Mapping;
using MajlesMefa.Back.UseCases.Commmands.AddCityCommand;
using IdentityContext;
using MajlesMefa.UI.Services;
using MajlesMefa.Back.Seeder;
using MajlesMefa.UI.Middleware;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using MajlesMefa.Back.Repositories.Abstraction;
using MajlesMefa.Back.Repositories.Implementation;
using NToastNotify;
using MajlesMefa.UI.Views.Home;
using MajlesMefa.Back.Utilities.FTP;
using MajlesMefa.Back.Repositories.Reddis;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddCurrentUserService();
builder.Services.AddHttpContextAccessor();
builder.Services.AddUnitOfWork();
//builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    // For development - allow any origin
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrEmpty(configuration))
    {
        configuration = "localhost:6379,abortConnect=false,connectTimeout=5000";
    }
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddCityCommand).Assembly));
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});
builder.Services.AddSession(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = false,
    PositionClass = ToastPositions.TopLeft
});

builder.Services.AddAntiforgery(opts =>
{
    opts.Cookie.Name = "_gui";
    opts.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opts.Cookie.SameSite = SameSiteMode.Strict;
    opts.Cookie.MaxAge = TimeSpan.FromMinutes(10);
    opts.Cookie.HttpOnly = true;
    opts.Cookie.IsEssential = true;
    opts.SuppressXFrameOptionsHeader = true;
});
builder.Services.AddMaper(typeof(DataEntryEntity).Assembly);
builder.Services.AddCustomIdentity<OptionService>(builder.Configuration, "AuthDb");
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IRedisRepository, RedisRepository>();

var app = builder.Build();
await app.Services.AddBaseUserSeed();
await app.Services.AddCitiesSeed();
await app.Services.AddPagesWithRoleAccessAsync(typeof(HomeController).Assembly);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    //app.UseExceptionHandler(errorApp =>
    //{
    //    errorApp.Run(async context =>
    //    {
    //        context.Response.StatusCode = 500;
    //        context.Response.ContentType = "application/json; charset=utf-8";
        
    //        var error = new
    //        {
    //            title = "Error",
    //            status = 500,
    //            instance = context.Request.Path,
    //            errors = (object)null
    //        };

    //        await context.Response.WriteAsync(JsonSerializer.Serialize(error,
    //            new JsonSerializerOptions 
    //            { 
    //                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) 
    //            }));
    //    });
    //});
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    await next();
});
app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseNToastNotify();
app.UseMiddleware<SessionValidationMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
