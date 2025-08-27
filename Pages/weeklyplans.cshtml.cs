using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learning_site.Pages
{
    // C# class to represent a single learning plan
    public class WeeklyTask
    {
        public string Title { get; set; } = string.Empty;
        public string ScheduledTime { get; set; } = string.Empty;
        public string VideoId { get; set; } = string.Empty; // YouTube video ID (e.g., 'dQw4w9WgXcQ')
        public bool IsCompleted { get; set; } = false;
        public string ThumbnailUrl => $"https://img.youtube.com/vi/{VideoId}/default.jpg";
    }

    public class weeklyplansModel : PageModel
    {
        private readonly Client _supabase;

        public weeklyplansModel(Client supabase)
        {
            _supabase = supabase;
        }

        public Dictionary<string, List<WeeklyTask>> WeeklyPlan { get; set; } = new Dictionary<string, List<WeeklyTask>>();

        public Task<IActionResult> OnGetAsync()
        {
            var session = _supabase.Auth.CurrentSession;
            if (session == null)
            {
                return Task.FromResult<IActionResult>(RedirectToPage("/login"));
            }

            // This is placeholder data. You will replace this with logic to fetch
            // the user's weekly plan from your Supabase database.
            WeeklyPlan = new Dictionary<string, List<WeeklyTask>>
            {
                {"Day 1", new List<WeeklyTask>
                    {
                        new WeeklyTask { Title = "C# Basics - Variables", ScheduledTime = "9:00 AM", VideoId = "M5JzD0lY6fM", IsCompleted = true },
                        new WeeklyTask { Title = "Object-Oriented Programming", ScheduledTime = "2:00 PM", VideoId = "g23Pj_wJ_L4" }
                    }
                },
                {"Day 2", new List<WeeklyTask>
                    {
                        new WeeklyTask { Title = "SQL Fundamentals", ScheduledTime = "10:30 AM", VideoId = "Hloq-E0Wz70" }
                    }
                },
                // Add more days as needed
            };
            return Task.FromResult<IActionResult>(Page());
        }
    }
}
