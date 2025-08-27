using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase;
using System.Threading.Tasks;
using System;
using Learning_site.Models;

namespace Learning_site.Pages
{
    public class SupabaseTestModel : PageModel
    {
        private readonly Client _supabase;

        public SupabaseTestModel(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task OnGet()
        {
            try
            {
                var response = await _supabase.From<Management>().Get();
                if (response.Models.Count > 0)
                {
                    ViewData["SupabaseResult"] = "Successfully connected to Supabase and fetched data from 'Management' table.";
                }
                else
                {
                    ViewData["SupabaseResult"] = "Successfully connected to Supabase, but 'Management' table is empty or does not exist.";
                }
            }
            catch (Exception ex)
            {
                ViewData["SupabaseResult"] = $"Error connecting to Supabase: {ex.Message}";
            }
        }
    }
}
