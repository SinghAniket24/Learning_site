using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Learning_site.Data;
using Learning_site.Services; // ✅ Add this for SupabaseService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Add your existing DbContext
builder.Services.AddDbContext<Learning_siteContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Learning_siteContext")
        ?? throw new InvalidOperationException("Connection string 'Learning_siteContext' not found.")));

// 👇 Register HttpClient factory
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

    options.CallbackPath = "/signin-auth0";
    options.ClaimsIssuer = "Auth0";
});

// 🔹 Enforce authentication globally
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// 🔹 Register SupabaseService
builder.Services.AddSingleton<SupabaseService>();

var app = builder.Build();

// 🔹 Initialize Supabase on startup
var supabaseService = app.Services.GetRequiredService<SupabaseService>();
await supabaseService.InitializeAsync();

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 🔹 Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
