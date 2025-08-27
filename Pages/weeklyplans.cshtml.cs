using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Collections.Generic;





namespace Learning_site.Pages.Plans

{

    public class WeeklyTask

    {

        public string Title { get; set; }

        public string ScheduledTime { get; set; }

        public string VideoId { get; set; } // YouTube video ID

        public bool IsCompleted { get; set; } = false;

        public string ThumbnailUrl => $"https://img.youtube.com/vi/{VideoId}/default.jpg";

    }





    public class WeeklyPlansModel : PageModel

    {

        public Dictionary<string, List<WeeklyTask>> WeeklyPlan { get; set; }





        public void OnGet()

        {

            // Placeholder data for now

            WeeklyPlan = new Dictionary<string, List<WeeklyTask>>

            {

                {"Day 1", new List<WeeklyTask>

                    {

new WeeklyTask { Title = "C# Basics - Variables", ScheduledTime = "9:00 AM", VideoId = "M5JzD0lY6fM", IsCompleted = true },

new WeeklyTask { Title = "Object-Oriented Programming", ScheduledTime = "2:00 PM", VideoId = "g23Pj_wJ_L4" }

                    }

                },

                {"Day 2", new List<WeeklyTask>

                    {

new WeeklyTask { Title = "SQL Fundamentals", ScheduledTime = "10:30 AM", VideoId = "Hloq-E0Wz70" }

                    }

                }

            };

        }

    }

}