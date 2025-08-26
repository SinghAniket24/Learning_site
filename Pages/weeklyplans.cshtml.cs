using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Learning_site.Pages.managements;

namespace Learning_site.Pages
{
    public class WeeklyPlansModel : PageModel
    {
        public Dictionary<string, List<WeeklyTask>> WeeklyPlan { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public IActionResult OnGet()
        {
            WeeklyPlan = new Dictionary<string, List<WeeklyTask>>();

            // 🔹 Determine whether it's a static or dynamic plan
            if (Id == 1) // static Python plan
            {
                WeeklyPlan["Day 1"] = new List<WeeklyTask>
                {
                    new WeeklyTask { Title = "Python Basics", VideoUrl = "https://www.youtube.com/watch?v=abc123" },
                    new WeeklyTask { Title = "Variables & Data Types", VideoUrl = "https://www.youtube.com/watch?v=def456" }
                };
            }
            else if (Id == 2) // static Web Dev plan
            {
                WeeklyPlan["Day 1"] = new List<WeeklyTask>
                {
                    new WeeklyTask { Title = "HTML Basics", VideoUrl = "https://www.youtube.com/watch?v=ghi789" },
                    new WeeklyTask { Title = "CSS Basics", VideoUrl = "https://www.youtube.com/watch?v=jkl012" }
                };
            }
            else
            {
                // Dynamic plans from CreateModel
                var planIndex = Id - 3;
                if (planIndex >= 0 && planIndex < CreateModel.MyPlansList.Count)
                {
                    var plan = CreateModel.MyPlansList[planIndex];

                    for (int i = 0; i < plan.DailyPlans.Count; i++)
                    {
                        var dayName = $"Day {i + 1}";
                        var tasks = plan.DailyPlans[i].Videos.Select(v => new WeeklyTask
                        {
                            Title = v.Title,
                            VideoUrl = v.VideoUrl,
                            ScheduledTime = "To be decided"
                        }).ToList();

                        WeeklyPlan[dayName] = tasks;
                    }
                }
                else
                {
                    WeeklyPlan["Info"] = new List<WeeklyTask>
                    {
                        new WeeklyTask { Title = "Plan not found", VideoUrl = "#" }
                    };
                }
            }

            return Page();
        }
    }

    public class WeeklyTask
    {
        public string Title { get; set; }
        public string ScheduledTime { get; set; }
        public string VideoUrl { get; set; }
        public bool IsCompleted { get; set; } = false;

        public string ThumbnailUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(VideoUrl) && VideoUrl.Contains("v="))
                {
                    var id = VideoUrl.Split("v=")[1].Split('&')[0];
                    return $"https://img.youtube.com/vi/{id}/default.jpg";
                }
                return "img/default.png";
            }
        }
    }
}
