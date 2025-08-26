using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Learning_site.Pages.managements;

namespace Learning_site.Pages
{
    public class SavePlanModel : PageModel
    {
        [BindProperty]
        public Plan PendingPlan { get; set; }

        public void OnGet()
        {
            if (TempData["PendingPlan"] != null)
            {
                PendingPlan = JsonSerializer.Deserialize<Plan>(TempData["PendingPlan"].ToString());
                TempData.Keep("PendingPlan"); // Keep data for postback
            }
        }

        public IActionResult OnPostSave()
        {
            if (TempData["PendingPlan"] != null)
            {
                var plan = JsonSerializer.Deserialize<Plan>(TempData["PendingPlan"].ToString());
                CreateModel.MyPlansList.Add(plan);  // ✅ Add to confirmed plans
            }

            return RedirectToPage("/MyPlans");
        }

        public IActionResult OnPostCancel()
        {
            TempData.Remove("PendingPlan");
            return RedirectToPage("/Create");
        }
    }
}
