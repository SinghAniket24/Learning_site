using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Learning_site.Pages
{
    public class CreatePlanModel : PageModel
    {
        private readonly YouTubePlanService _ytService = new YouTubePlanService();

        [BindProperty]
        public PlanData Plan { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // 1. Fetch videos from YouTube
            var videos = await _ytService.SearchVideosAsync(Plan.Title, Plan.Channel);

            // 2. Generate daily plan
            var dailyPlan = _ytService.CreateDailyPlan(videos, Plan.Days, Plan.DailyTime);

            // 3. Temporarily store in session/static memory and go to SavePlan page
            TempPlanStore.TempPlan = dailyPlan;
            TempPlanStore.PlanInfo = Plan;

            return RedirectToPage("/SavePlan"); // Navigate to save confirmation page
        }
    }

    public class PlanData
    {
        public string Title { get; set; }
        public string Channel { get; set; }
        public string Description { get; set; }
        public double DailyTime { get; set; }
        public int Days { get; set; }
    }

    // Temporary storage for generated plan (before saving)
    public static class TempPlanStore
    {
        public static List<DailyPlan> TempPlan { get; set; }
        public static PlanData PlanInfo { get; set; }
    }

    // Final storage after user clicks Save
    public static class SavedPlansStore
    {
        public static List<(PlanData Info, List<DailyPlan> Plan)> SavedPlans
            = new List<(PlanData, List<DailyPlan>)>();
    }
}
