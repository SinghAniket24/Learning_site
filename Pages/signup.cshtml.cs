using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Required for ILogger

namespace Learning_site.Pages
{
    public class signupModel : PageModel
    {
        private readonly Supabase.Client _supabase;
        private readonly ILogger<signupModel> _logger; // Logger for this page

        public signupModel(Supabase.Client supabase, ILogger<signupModel> logger)
        {
            _supabase = supabase;
            _logger = logger; // Inject the logger
        }

        [BindProperty, Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [BindProperty, Required, Range(1, 120)]
        public int Age { get; set; }

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BindProperty, Required, DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [TempData]
        public string? ErrorMessage { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
            _logger.LogInformation("Signup page loaded (OnGet).");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // DEBUGGING: Log when the signup form is submitted
            _logger.LogInformation("OnPostAsync triggered for signup.");
            _logger.LogInformation("Attempting signup for email: {Email}", Email);

            if (!ModelState.IsValid)
            {
                // DEBUGGING: Log if the model validation fails
                _logger.LogWarning("ModelState is invalid. Returning Page.");
                return Page();
            }

            try
            {
                var session = await _supabase.Auth.SignUp(Email, Password, new SignUpOptions
                {
                    Data = new System.Collections.Generic.Dictionary<string, object>
                    {
                        { "username", Username },
                        { "age", Age }
                    }
                });

                if (session?.User != null)
                {
                    // DEBUGGING: Log for the case where email confirmation is disabled
                    _logger.LogInformation("Signup successful and session received (Email Confirmation likely DISABLED). User ID: {UserId}", session.User.Id);
                    SuccessMessage = "Signup successful! Please check your email for a confirmation link.";
                    return RedirectToPage();
                }
                else
                {
                    // DEBUGGING: Log for the case where email confirmation is enabled
                    _logger.LogInformation("Signup call successful (Session is null, Email Confirmation likely ENABLED). A confirmation email has been sent.");
                    SuccessMessage = "Signup successful! Please check your email for a confirmation link.";
                    return RedirectToPage();
                }
            }
            catch (GotrueException ex)
            {
                // DEBUGGING: Log specific Supabase authentication errors
                _logger.LogError(ex, "A GotrueException occurred during signup for {Email}", Email);
                ErrorMessage = $"Sign up failed: {ex.Message}";
                return Page();
            }
            catch (Exception ex)
            {
                // DEBUGGING: Log any other unexpected errors
                _logger.LogError(ex, "An unexpected error occurred during signup for {Email}", Email);
                ErrorMessage = "An unexpected error occurred. Please try again.";
                return Page();
            }
        }
    }
}