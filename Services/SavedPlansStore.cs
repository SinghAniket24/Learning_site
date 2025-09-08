using Learning_site.Models.Entities;
using Learning_site.Pages; 
using Supabase;
using Postgrest;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using static Supabase.Postgrest.Constants;

namespace Learning_site.Services
{
    public static class SavedPlansStore
    {
    
        public static List<(PlanData, List<DailyPlan>)> SavedPlans { get; private set; } = new();

        public static async Task LoadUserPlansAsync(SupabaseService supabaseService, string userId)
        {
            if (supabaseService == null || string.IsNullOrWhiteSpace(userId))
                return;

            SavedPlans.Clear();


            var planResponse = await supabaseService.Client
                .From<PlanEntity>()
                .Filter("user_id", Operator.Equals, userId)
                .Get();

            var sortedPlans = planResponse?.Models ?? new List<PlanEntity>();
            sortedPlans.Sort((a, b) => a.Id.CompareTo(b.Id));

            foreach (var planEntity in sortedPlans)
            {
               
                var dailyResponse = await supabaseService.Client
                    .From<DailyPlanEntity>()
                    .Filter("plan_id", Operator.Equals, planEntity.Id)
                    .Get();

                var dailyPlans = new List<DailyPlan>();

                foreach (var d in dailyResponse?.Models ?? new List<DailyPlanEntity>())
                {
                    List<VideoData> videos;

                    try
                    {
                        videos = string.IsNullOrWhiteSpace(d.Videos)
                            ? new List<VideoData>()
                            : JsonSerializer.Deserialize<List<VideoData>>(d.Videos) ?? new List<VideoData>();
                    }
                    catch
                    {
              
                        videos = new List<VideoData>();
                    }

                    dailyPlans.Add(new DailyPlan
                    {
                        Day = d.Day,
                        Videos = videos
                    });
                }

           
                SavedPlans.Add((
                    new PlanData
                    {
                        Title = planEntity.Title,
                        Description = planEntity.Description,
                        DailyTime = planEntity.DailyTime,
                        Days = planEntity.Days
                    },
                    dailyPlans
                ));
            }
        }

        public static void AddPlan(PlanData planData, List<DailyPlan> dailyPlans)
        {
            if (planData != null && dailyPlans != null)
            {
                SavedPlans.Add((planData, dailyPlans));
            }
        }
    }
}
