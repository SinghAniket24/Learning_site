using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Learning_site.Pages
{
    public class SavePlanModel : PageModel
    {
        public PlanData PlanInfo { get; set; }
        public List<DailyPlan> DailyPlan { get; set; }

        public void OnGet()
        {
            PlanInfo = TempPlanStore.PlanInfo;
            DailyPlan = TempPlanStore.TempPlan;
        }

        public IActionResult OnPost()
        {
            if (TempPlanStore.PlanInfo != null && TempPlanStore.TempPlan != null)
            {
                SavedPlansStore.SavedPlans.Add((TempPlanStore.PlanInfo, TempPlanStore.TempPlan));

                // Clear temporary storage
                TempPlanStore.PlanInfo = null;
                TempPlanStore.TempPlan = null;

                TempData["SuccessMessage"] = "Plan saved successfully!";
            }

            return RedirectToPage("/MyPlans");
        }
    }
}
