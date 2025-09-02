using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Learning_site.Pages
{
    // Classes for recommended plans (same structure as dashboard)
    public class RecVideo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelName { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Duration { get; set; }
        public int StartTime { get; set; }
    }

    public class RecDayPlan
    {
        public int DayNumber { get; set; }
        public List<RecVideo> Videos { get; set; }
    }

    public class RecPlan
    {
        public string Title { get; set; }
        public string Channel { get; set; }
        public int TotalDays { get; set; }
        public List<RecDayPlan> DayPlans { get; set; }
    }

    public class RecommendedPlanDetailsModel : PageModel
    {
        public string PlanTitle { get; set; }
        public List<RecDayPlan> DayPlans { get; set; }

        public IActionResult OnGet(string planTitle)
        {
            if (string.IsNullOrEmpty(planTitle))
                return NotFound();

            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "recommended_plans.json");
            if (!System.IO.File.Exists(jsonFilePath))
                return NotFound();

            var json = System.IO.File.ReadAllText(jsonFilePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var rawPlans = JsonSerializer.Deserialize<List<RawRecPlan>>(json, options);

            var selectedPlan = rawPlans
                .Where(p => p.Title == planTitle)
                .Select(p => new RecPlan
                {
                    Title = p.Title,
                    Channel = p.Channel,
                    TotalDays = p.TotalDays,
                    DayPlans = p.DailyPlan.Select(d => new RecDayPlan
                    {
                        DayNumber = d.Day,
                        Videos = d.Videos.Select(v => new RecVideo
                        {
                            Title = v.Title,
                            Description = v.Description,
                            ChannelName = v.ChannelName,
                            ThumbnailUrl = v.ThumbnailUrl,
                            VideoUrl = v.VideoUrl,
                            Duration = v.Duration,
                            StartTime = v.StartTime
                        }).ToList()
                    }).ToList()
                }).FirstOrDefault();

            if (selectedPlan == null)
                return NotFound();

            PlanTitle = selectedPlan.Title;
            DayPlans = selectedPlan.DayPlans;

            return Page();
        }

        // Helper classes for JSON deserialization
        private class RawRecPlan
        {
            public string Title { get; set; }
            public string Channel { get; set; }
            public int TotalDays { get; set; }
            public List<RawRecDayPlan> DailyPlan { get; set; }
        }

        private class RawRecDayPlan
        {
            public int Day { get; set; }
            public List<RecVideo> Videos { get; set; }
        }
    }
}
