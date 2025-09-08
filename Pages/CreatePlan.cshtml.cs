using Learning_site.Services;
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

     
            var videos = await _ytService.SearchVideosAsync(Plan.Title, Plan.Channel);

            
            var dailyPlan = _ytService.CreateDailyPlan(videos, Plan.Days, Plan.DailyTime);

          
            TempPlanStore.TempPlan = dailyPlan;
            TempPlanStore.PlanInfo = Plan;

            return RedirectToPage("/SavePlan");
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


    public static class TempPlanStore
    {
        public static List<DailyPlan> TempPlan { get; set; }
        public static PlanData PlanInfo { get; set; }
    }


   
}
