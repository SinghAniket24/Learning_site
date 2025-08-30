using Learning_site.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<Learning_siteContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Learning_siteContext")
        ?? throw new InvalidOperationException("Connection string 'Learning_siteContext' not found.")));
// 👇 Register HttpClient factory
builder.Services.AddHttpClient();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// Redirect root (/) url to /Landing
app.MapGet("/", context =>
{
    context.Response.Redirect("/Landing");
    return Task.CompletedTask;
});

app.Run();
