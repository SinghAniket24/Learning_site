using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Learning_site.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace Learning_site.Pages.managements
{
    public class CreateModel : PageModel
    {
        private readonly Learning_siteContext _context;
        private readonly YouTubePlanService _youtubeService;

        public CreateModel(Learning_siteContext context)
        {
            _context = context;
            _youtubeService = new YouTubePlanService();
        }

        [BindProperty]
        public Management management { get; set; } = new Management();

        // In-memory storage for confirmed plans (optional, can remove if not needed)
        public static List<Plan> MyPlansList { get; set; } = new List<Plan>();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validation: stay on form if errors
            if (!ModelState.IsValid)
                return Page();

            // Fetch videos from YouTube
            var videos = await _youtubeService.SearchVideosAsync(management.Title, management.Channel);

            // Create daily plan
            var dailyPlans = _youtubeService.CreateDailyPlan(videos, management.Days, management.DailyTime);

            // Create plan object
            var plan = new Plan
            {
                Title = management.Title,
                Topic = management.Description,
                DailyPlans = dailyPlans
            };

            // Save the form data into DB
            var dbManagement = new Models.management
            {
                Title = management.Title,
                Channel = management.Channel,
                Description = management.Description,
                DailyTime = management.DailyTime,
                Days = management.Days
            };
            _context.management.Add(dbManagement);
            await _context.SaveChangesAsync();

            // Save the generated plan in TempData (for use in SavePlan page)
            TempData["PendingPlan"] = JsonSerializer.Serialize(plan);

            // Redirect strictly to SavePlan!
            return RedirectToPage("/SavePlan");
        }
    }

    // DTOs
    public class Plan
    {
        public string Title { get; set; }
        public string Topic { get; set; }
        public List<DailyPlan> DailyPlans { get; set; }
    }

    public class DailyPlan
    {
        public int DayNumber { get; set; }
        public List<VideoData> Videos { get; set; }
    }

    public class VideoData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelName { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Duration { get; set; }
    }

    public class Management
    {
        public string Title { get; set; }
        public string Channel { get; set; }
        public string Description { get; set; }
        public double DailyTime { get; set; }
        public int Days { get; set; }
    }
}
