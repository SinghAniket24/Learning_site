// Pages/Api/AuthController.cs

using Microsoft.AspNetCore.Mvc;
using Supabase;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly Client _supabase;

    public AuthController(Client supabase)
    {
        _supabase = supabase;
    }

    [HttpPost("setsession")]
    public async Task<IActionResult> SetSession([FromBody] SessionModel model)
    {
        if (string.IsNullOrEmpty(model.AccessToken) || string.IsNullOrEmpty(model.RefreshToken))
        {
            return BadRequest("Access Token and Refresh Token are required.");
        }

        // Set the session on the server-side Supabase client
        await _supabase.Auth.SetSession(model.AccessToken, model.RefreshToken);

        return Ok(new { message = "Session set successfully." });
    }
}

// A simple model to receive the session data
public class SessionModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}