using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase;
using System.Collections.Generic;
using System.Threading.Tasks;

//namespace YourAppNamespace.Pages
namespace Learning_site.Pages

{
    public class MyPlansModel : PageModel
    {
        private readonly Client _supabase;

        public MyPlansModel(Client supabase)
        {
            _supabase = supabase;
        }

        // This will hold all the user's study plans
        public List<StudyPlan> Plans { get; set; } = new List<StudyPlan>();

        public Task<IActionResult> OnGetAsync()
        {
            var session = _supabase.Auth.CurrentSession;
            if (session == null)
            {
                return Task.FromResult<IActionResult>(RedirectToPage("/login"));
            }

            // 🔹 For now, let's mock some data (later you’ll fetch from DB)
            Plans = new List<StudyPlan>
            {
                new StudyPlan {
                    Id = 1,
                    Name = "Python for Beginners",
                    ChannelAvatarUrl = "img/python.png",
                    CompletedLessons = 3,
                    TotalLessons = 10
                },
                new StudyPlan {
                    Id = 2,
                    Name = "Web Development Basics",
                    ChannelAvatarUrl = "img/web.jpeg",
                    CompletedLessons = 5,
                    TotalLessons = 12
                }
            };
            return Task.FromResult<IActionResult>(Page());
        }
    }

    // Simple model for now
    public class StudyPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ChannelAvatarUrl { get; set; } = string.Empty;
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
    }
}