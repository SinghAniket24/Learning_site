using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Learning_site.Services;


namespace Learning_site.Pages
{
    public class WeeklyPlansModel : PageModel
    {
        public int PlanId { get; set; }
        public string PlanTitle { get; set; }
        public List<DailyPlan> DailyPlan { get; set; }

        public IActionResult OnGet(int id)
        {
            if (id <= 0 || id > SavedPlansStore.SavedPlans.Count)
                return NotFound();

            var saved = SavedPlansStore.SavedPlans[id - 1];
            PlanId = id;
            PlanTitle = saved.Item1.Title;
            DailyPlan = saved.Item2; // ✅ Using DailyPlan directly

            return Page();
        }
    }
}
