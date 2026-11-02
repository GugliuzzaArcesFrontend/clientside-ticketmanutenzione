using clientside_ticketmanutenzione.Components;
using clientside_ticketmanutenzione.Endpoints;
using clientside_ticketmanutenzione.Handlers;
using clientside_ticketmanutenzione.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using System.Security.Claims;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
    .CreateBootstrapLogger();
try
{

    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    builder.Services.AddMemoryCache();

    builder.Services.AddTransient<AuthTokenHandler>();
    builder.Services.AddHttpClient("apiLogin", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
    });
    builder.Services
        .AddHttpClient("api_test1", client =>
        {
            client.BaseAddress = new Uri(
                builder.Configuration["ApiBaseUrl"]!); //impostazione dell'origine api, presa da appsettings
            client.Timeout = TimeSpan.FromSeconds(30); //settaggio del request timeout 408/504
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

    builder.Services
        .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "__Host-api-test1";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.Path = "/";
            options.Cookie.MaxAge = TimeSpan.FromHours(8);

            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = false;

            options.LoginPath = "/login";
            options.AccessDeniedPath = "/denied";

            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            };

            options.Events.OnValidatePrincipal = async context =>
            {
                var sessionManager = context.HttpContext
                    .RequestServices
                    .GetRequiredService<AuthSessionManager>();

                var sessionId = context.Principal?
                    .FindFirst("sid")?.Value;

                if (string.IsNullOrWhiteSpace(sessionId) ||
                    sessionManager.GetSession(sessionId) is null)
                {
                    context.RejectPrincipal();

                    await context.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                }
            };
        });

    builder.Services.AddAuthorization();
    builder.Services.AddAuthorizationCore();
    builder.Services.AddCascadingAuthenticationState();

    //spazio per i servizi singleton

    builder.Services.AddSingleton<AuthSessionManager>();

    //spazio per i servizi scoped

    builder.Services.AddScoped<AuthService>();

    //build
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();
    app.MapAuthenticationEndpoints();
    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal("Errore critico: {message} \n {inner}",ex.Message, ex.InnerException);
}
finally
{
    await Log.CloseAndFlushAsync();
}