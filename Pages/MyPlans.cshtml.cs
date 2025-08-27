using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;



//namespace YourAppNamespace.Pages
namespace Learning_site.Pages

{
    public class MyPlansModel : PageModel
    {
        // This will hold all the user's study plans
        public List<StudyPlan> Plans { get; set; }

        public void OnGet()
        {
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
        }
    }

    // Simple model for now
    public class StudyPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChannelAvatarUrl { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
    }
}