using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Learning_site.Pages
{
    public class MyPlansModel : PageModel
    {
        public List<StudyPlan> Plans { get; set; }

        public void OnGet()
        {
            // Load all saved plans from SavedPlansStore
            Plans = SavedPlansStore.SavedPlans.Select((p, index) => new StudyPlan
            {
                Id = index + 1,
                Name = p.Item1.Title,
                IconUrl = GetTopicIcon(p.Item1.Title),
                CompletedLessons = 0,
                TotalLessons = p.Item2.Sum(d => d.Videos.Count)
            }).ToList();
        }

        // Generate meaningful icon dynamically based on topic name
        private string GetTopicIcon(string topic)
        {
            topic = topic.ToLower();

            // Popular tech/dev topics → use Devicon
            var techMap = new[] { "python", "java", "javascript", "html", "css", "react", "node", "csharp", "php", "sql" };
            foreach (var tech in techMap)
            {
                if (topic.Contains(tech))
                    return $"https://cdn.jsdelivr.net/gh/devicons/devicon/icons/{tech}/{tech}-original.svg";
            }

            // Otherwise use Flaticon general education icon
            return "https://cdn-icons-png.flaticon.com/512/3135/3135715.png";
        }
    }

    public class StudyPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
    }

}
