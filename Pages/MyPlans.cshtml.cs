using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Learning_site.Pages.managements;

namespace Learning_site.Pages
{
    public class MyPlansModel : PageModel
    {
        public List<StudyPlan> Plans { get; set; }

        public void OnGet()
        {
            Plans = new List<StudyPlan>();

            // 🔹 Static mock plans
            Plans.Add(new StudyPlan
            {
                Id = 1,
                Name = "Python for Beginners",
                ChannelAvatarUrl = "img/python.png",
                CompletedLessons = 3,
                TotalLessons = 10
            });

            Plans.Add(new StudyPlan
            {
                Id = 2,
                Name = "Web Development Basics",
                ChannelAvatarUrl = "img/web.jpeg",
                CompletedLessons = 5,
                TotalLessons = 12
            });

            // 🔹 Dynamic plans from CreateModel
            int nextId = 3;
            foreach (var plan in CreateModel.MyPlansList)
            {
                Plans.Add(new StudyPlan
                {
                    Id = nextId++,
                    Name = plan.Title,
                    ChannelAvatarUrl = "img/default.png",
                    CompletedLessons = 0,
                    TotalLessons = plan.DailyPlans.Count
                });
            }
        }
    }

    public class StudyPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChannelAvatarUrl { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
    }
}
