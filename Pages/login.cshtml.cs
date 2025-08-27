using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase.Gotrue.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Learning_site.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Supabase.Client _supabase;
        private readonly ILogger<LoginModel> _logger; // It's good practice to add logging

        public LoginModel(Supabase.Client supabase, ILogger<LoginModel> logger)
        {
            _supabase = supabase;
            _logger = logger;
        }

        [BindProperty, EmailAddress, Required]
        public string Email { get; set; } = string.Empty;

        [BindProperty, DataType(DataType.Password), Required]
        public string Password { get; set; } = string.Empty;

        [TempData]
        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _logger.LogInformation("Attempting to sign in user {Email}", Email);
                var session = await _supabase.Auth.SignIn(Email, Password);

                if (session?.User != null)
                {
                    _logger.LogInformation("User {Email} signed in successfully on the server.", Email);
                    // Successful login. Redirect back to this same page.
                    // The onAuthStateChange listener in site.js will detect the
                    // new session and handle the redirect to the dashboard.
                    return RedirectToPage(); // <-- THIS IS THE ONLY CHANGE
                }

                ErrorMessage = "An unknown error occurred.";
                return Page();
            }
            catch (GotrueException ex)
            {
                _logger.LogWarning(ex, "Login failed for user {Email}", Email);
                ErrorMessage = $"Login failed: {ex.Message}";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login for {Email}", Email);
                ErrorMessage = "An unexpected error occurred. Please try again.";
                return Page();
            }
        }
    }
}