<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Learning_site.Data;
using Learning_site.Services; // ✅ SupabaseService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Add your DbContext
=======
﻿using Learning_site.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
>>>>>>> 79a743748de311cf316d63fcc5ffa941cdda5eca
builder.Services.AddDbContext<Learning_siteContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Learning_siteContext")
        ?? throw new InvalidOperationException("Connection string 'Learning_siteContext' not found.")));
<<<<<<< HEAD

// HttpClient factory
=======
// 👇 Register HttpClient factory
>>>>>>> 79a743748de311cf316d63fcc5ffa941cdda5eca
builder.Services.AddHttpClient();

// 🔹 Configure Authentication (Auth0 + Cookies)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // "Cookies"
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; // "OpenIdConnect"
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    options.ResponseType = "code";

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.CallbackPath = "/signin-auth0"; // login callback
    options.ClaimsIssuer = "Auth0";

    // 🔹 Handle logout redirect
    options.SignedOutCallbackPath = "/signout-callback-oidc"; // local endpoint for post-logout

    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProviderForSignOut = context =>
        {
            // Tell Auth0 where to send the user after logout
            var logoutUri = $"https://{builder.Configuration["Auth0:Domain"]}/v2/logout?client_id={builder.Configuration["Auth0:ClientId"]}";

            // Absolute redirect after logout
            var postLogoutUri = context.Request.Scheme + "://" + context.Request.Host + "/dashboard";
            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";

            context.Response.Redirect(logoutUri);
            context.HandleResponse();

            return Task.CompletedTask;
        }
    };
});

// 🔹 Enforce authentication globally
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// Supabase service
builder.Services.AddSingleton<SupabaseService>();

var app = builder.Build();
<<<<<<< HEAD

// 🔹 Initialize Supabase on startup
var supabaseService = app.Services.GetRequiredService<SupabaseService>();
await supabaseService.InitializeAsync();

// Configure HTTP request pipeline
=======
// Configure the HTTP request pipeline.
>>>>>>> 79a743748de311cf316d63fcc5ffa941cdda5eca
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
<<<<<<< HEAD

// Auth before Authorization
app.UseAuthentication();
=======
>>>>>>> 79a743748de311cf316d63fcc5ffa941cdda5eca
app.UseAuthorization();

app.MapRazorPages();

// Redirect root (/) url to /Landing
app.MapGet("/", context =>
{
    context.Response.Redirect("/Landing");
    return Task.CompletedTask;
});

app.Run();
