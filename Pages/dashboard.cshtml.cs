using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Learning_site.Pages
{
    // C# class to represent a single learning plan
    public class LearningPlan
    {
        public string Title { get; set; } = string.Empty;
        public int TotalVideos { get; set; }
        public int WatchedVideos { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;
        public int Progress => TotalVideos > 0 ? (int)Math.Round((double)WatchedVideos / TotalVideos * 100) : 0;
    }

    // C# class to deserialize the Zen Quotes API response
    public class ZenQuote
    {
        [JsonPropertyName("q")]
        public string QuoteText { get; set; } = string.Empty;
        [JsonPropertyName("a")]
        public string Author { get; set; } = string.Empty;
    }

    public class dashboardModel : PageModel
    {
        // Public properties to hold data that the view will display
        public string UserName { get; set; } = "Alex";
        public string MotivationalQuote { get; set; } = string.Empty;
        public string QuoteAuthor { get; set; } = string.Empty;
        public List<LearningPlan> CurrentPlans { get; set; } = new List<LearningPlan>();

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Client _supabase;

        // Constructor to inject IHttpClientFactory and Supabase Client services
        public dashboardModel(IHttpClientFactory httpClientFactory, Client supabase)
        {
            _httpClientFactory = httpClientFactory;
            _supabase = supabase;
        }

        // This method runs when the page is requested
        public async Task<IActionResult> OnGetAsync()
        {
            var session = _supabase.Auth.CurrentSession;
            if (session == null)
            {
                return RedirectToPage("/login");
            }

            // Fetch the quote from the API
            await GetRandomQuoteAsync();

            // Simulate fetching data from your Supabase database
            CurrentPlans = GetUserLearningPlansFromDatabase();
            return Page();
        }

        // Method to call the motivational quotes API
        private async Task GetRandomQuoteAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<ZenQuote>>("https://zenquotes.io/api/random");
                if (response != null && response.Count > 0)
                {
                    var quote = response[0];
                    MotivationalQuote = quote.QuoteText;
                    QuoteAuthor = quote.Author;
                }
            }
            catch (Exception ex)
            {
                // Set a default quote if the API call fails
                MotivationalQuote = "The journey of a thousand miles begins with a single step.";
                QuoteAuthor = "Lao Tzu";
                Console.WriteLine($"Error fetching quote: {ex.Message}");
            }
        }

        // Placeholder method for getting user data
        private List<LearningPlan> GetUserLearningPlansFromDatabase()
        {
            // Replace this with your actual Supabase data fetching logic
            return new List<LearningPlan>
            {
                new LearningPlan { Title = "Python Basics", TotalVideos = 12, WatchedVideos = 9, ThumbnailUrl = "/images/python-logo.png" },
                new LearningPlan { Title = "Web Development Fundamentals", TotalVideos = 25, WatchedVideos = 10, ThumbnailUrl = "/images/web-dev-logo.png" },
                new LearningPlan { Title = "Data Science Essentials", TotalVideos = 20, WatchedVideos = 2, ThumbnailUrl = "/images/data-science-logo.png" }
            };
        }
    }
}
