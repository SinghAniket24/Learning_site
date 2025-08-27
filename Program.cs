using Microsoft.EntityFrameworkCore;
using Learning_site.Data;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// DEBUGGING: Get a logger to print startup messages
var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("Startup");


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // <-- ADD THIS LINE
builder.Services.AddDbContext<Learning_siteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Learning_siteContext") ?? throw new InvalidOperationException("Connection string 'Learning_siteContext' not found.")));
builder.Services.AddHttpClient();

var supabaseUrl = builder.Configuration["SUPABASE_URL"];
var supabaseKey = builder.Configuration["SUPABASE_KEY"];

// DEBUGGING: Log the Supabase URL to confirm it's loaded.
// WARNING: Do not log the full supabaseKey in a production environment.
logger.LogInformation("--- App Startup ---");
logger.LogInformation("Supabase URL loaded: {SupabaseUrl}", supabaseUrl);
logger.LogInformation("Supabase Key loaded: {HasKey}", !string.IsNullOrEmpty(supabaseKey));


if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
{
    logger.LogError("Supabase URL or Key not found in configuration. Application cannot start.");
    throw new InvalidOperationException("Supabase URL and Key must be set in .env file.");
}

builder.Services.AddSingleton(new Supabase.Client(
    supabaseUrl,
    supabaseKey,
    new Supabase.SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    }));

logger.LogInformation("Supabase client registered as a singleton service.");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    app.MapControllers(); // <-- ADD THIS LINE
    endpoints.MapFallbackToPage("/login");
});

logger.LogInformation("Application is starting up...");
app.Run();