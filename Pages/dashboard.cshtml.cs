using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Learning_site.Pages
{
    public class DashVideo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelName { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Duration { get; set; }
        public int StartTime { get; set; }
    }

    public class DashDayPlan
    {
        public int DayNumber { get; set; }
        public List<DashVideo> Videos { get; set; }
    }

    public class DashRecommendedPlan
    {
        public string Title { get; set; }
        public string Channel { get; set; }
        public int TotalDays { get; set; }
        public List<DashDayPlan> DayPlans { get; set; }
    }

    public class ZenQuote
    {
        [JsonPropertyName("q")]
        public string QuoteText { get; set; }
        [JsonPropertyName("a")]
        public string Author { get; set; }
    }

    public class dashboardModel : PageModel
    {
        public string UserName { get; set; } = "Guest";
        public string UserPictureUrl { get; set; }
        public string MotivationalQuote { get; set; }
        public string QuoteAuthor { get; set; }

        public List<DashRecommendedPlan> RecommendedPlans { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public dashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                UserName = User.FindFirst("name")?.Value
                           ?? User.FindFirst("nickname")?.Value
                           ?? User.Identity.Name
                           ?? "User";

                UserPictureUrl = User.FindFirst("picture")?.Value;
            }

            await GetRandomQuoteAsync();
            LoadRecommendedPlans();
        }

        private async Task GetRandomQuoteAsync()
        {
            // List of 8 education-focused quotes
            var educationQuotes = new List<ZenQuote>
    {
        new ZenQuote { QuoteText = "Education is the most powerful weapon which you can use to change the world.", Author = "Nelson Mandela" },
        new ZenQuote { QuoteText = "The beautiful thing about learning is that nobody can take it away from you.", Author = "B.B. King" },
        new ZenQuote { QuoteText = "Live as if you were to die tomorrow. Learn as if you were to live forever.", Author = "Mahatma Gandhi" },
        new ZenQuote { QuoteText = "An investment in knowledge pays the best interest.", Author = "Benjamin Franklin" },
        new ZenQuote { QuoteText = "Tell me and I forget. Teach me and I remember. Involve me and I learn.", Author = "Benjamin Franklin" },
        new ZenQuote { QuoteText = "Education is not preparation for life; education is life itself.", Author = "John Dewey" },
        new ZenQuote { QuoteText = "The mind is not a vessel to be filled, but a fire to be ignited.", Author = "Plutarch" },
        new ZenQuote { QuoteText = "Intellectual growth should commence at birth and cease only at death.", Author = "Albert Einstein" }
    };

            try
            {
                // Simulate async work (to keep method signature async)
                await Task.Delay(50);

                var random = new Random();
                var selectedQuote = educationQuotes[random.Next(educationQuotes.Count)];
                MotivationalQuote = selectedQuote.QuoteText;
                QuoteAuthor = selectedQuote.Author;
            }
            catch
            {
                // Fallback quote in case something goes wrong
                MotivationalQuote = "The journey of a thousand miles begins with a single step.";
                QuoteAuthor = "Lao Tzu";
            }
        }


        private void LoadRecommendedPlans()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "recommended_plans.json");
            if (System.IO.File.Exists(jsonFilePath))
            {
                var json = System.IO.File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var rawPlans = JsonSerializer.Deserialize<List<RawPlan>>(json, options);

                RecommendedPlans = rawPlans.Select(p => new DashRecommendedPlan
                {
                    Title = p.Title,
                    Channel = p.Channel,
                    TotalDays = p.TotalDays,
                    DayPlans = p.DailyPlan.Select(d => new DashDayPlan
                    {
                        DayNumber = d.Day,
                        Videos = d.Videos.Select(v => new DashVideo
                        {
                            Title = v.Title,
                            Description = v.Description,
                            ChannelName = v.ChannelName,
                            ThumbnailUrl = v.ThumbnailUrl,
                            VideoUrl = v.VideoUrl,
                            Duration = v.Duration,
                            StartTime = v.StartTime
                        }).ToList()
                    }).ToList()
                }).ToList();
            }
            else
            {
                RecommendedPlans = new List<DashRecommendedPlan>();
            }
        }

        private class RawPlan
        {
            public string Title { get; set; }
            public string Channel { get; set; }
            public int TotalDays { get; set; }
            public List<RawDayPlan> DailyPlan { get; set; }
        }

        private class RawDayPlan
        {
            public int Day { get; set; }
            public List<DashVideo> Videos { get; set; }
        }
    }
}
