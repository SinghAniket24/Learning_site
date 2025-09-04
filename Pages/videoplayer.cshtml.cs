using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Learning_site.Services;


namespace Learning_site.Pages
{
    public class VideoPlayerModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string VideoId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PlanId { get; set; }

        public string VideoTitle { get; set; }
        public string ChannelName { get; set; }
        public string Duration { get; set; }

        public IActionResult OnGet(string videoId, int planId)
        {
            VideoId = videoId;
            PlanId = planId;

            var plan = SavedPlansStore.SavedPlans.ElementAtOrDefault(planId - 1);
            if (plan.Item2 == null) return NotFound();

            var video = plan.Item2
                           .SelectMany(d => d.Videos)
                           .FirstOrDefault(v =>
                           {
                               var urlParts = v.VideoUrl.Split("id=");
                               return urlParts.Length > 1 && urlParts[1] == videoId;
                           });

            if (video == null) return NotFound();

            VideoTitle = video.Title;
            ChannelName = video.ChannelName;
            Duration = video.Duration; // hh:mm:ss

            return Page();
        }
    }
}