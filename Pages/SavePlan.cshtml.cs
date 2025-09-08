using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Learning_site.Models.Entities;
using Learning_site.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Learning_site.Pages
{
    public class SavePlanModel : PageModel
    {
        private readonly SupabaseService _supabase;

        public SavePlanModel(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        public PlanData PlanInfo { get; set; }
        public List<DailyPlan> DailyPlan { get; set; }

        public void OnGet()
        {
            PlanInfo = TempPlanStore.PlanInfo;
            DailyPlan = TempPlanStore.TempPlan;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (TempPlanStore.PlanInfo == null || TempPlanStore.TempPlan == null)
                return BadRequest("No plan data to save.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var planEntity = new PlanEntity
            {
                UserId = userId,
                Title = TempPlanStore.PlanInfo.Title,
                Description = TempPlanStore.PlanInfo.Description,
                DailyTime = TempPlanStore.PlanInfo.DailyTime,
                Days = TempPlanStore.PlanInfo.Days
            };

            var insertResponse = await _supabase.Client
                .From<PlanEntity>()
                .Insert(planEntity);

            var insertedPlan = insertResponse.Models.Count > 0 ? insertResponse.Models[0] : null;
            if (insertedPlan == null)
                return StatusCode(500, "Failed to save plan.");

            
            foreach (var day in TempPlanStore.TempPlan)
            {
                var dailyEntity = new DailyPlanEntity
                {
                    PlanId = insertedPlan.Id,
                    Day = day.Day,
                    Videos = JsonSerializer.Serialize(day.Videos)
                };
                await _supabase.Client.From<DailyPlanEntity>().Insert(dailyEntity);
            }

            TempPlanStore.PlanInfo = null;
            TempPlanStore.TempPlan = null;

   
            await SavedPlansStore.LoadUserPlansAsync(_supabase, userId);

            TempData["SuccessMessage"] = "Plan saved successfully!";

            return RedirectToPage("/MyPlans");
        }
    }
}
