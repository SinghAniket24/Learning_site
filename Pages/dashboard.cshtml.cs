using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Learning_site.Pages
{
    // C# class to represent a single learning plan
    public class LearningPlan
    {
        public string Title { get; set; }
        public int TotalVideos { get; set; }
        public int WatchedVideos { get; set; }
        public string ThumbnailUrl { get; set; }
        public int Progress => TotalVideos > 0 ? (int)Math.Round((double)WatchedVideos / TotalVideos * 100) : 0;
    }

    // C# class to deserialize the Zen Quotes API response
    public class ZenQuote
    {
        [JsonPropertyName("q")]
        public string QuoteText { get; set; }
        [JsonPropertyName("a")]
        public string Author { get; set; }
    }

    public class dashboardModel : PageModel
    {
        // Public properties to hold data that the view will display
        public string UserName { get; set; } = "Alex";
        public string MotivationalQuote { get; set; }
        public string QuoteAuthor { get; set; }
        public List<LearningPlan> CurrentPlans { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        // Constructor to inject IHttpClientFactory service
        public dashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // This method runs when the page is requested
        public async Task OnGetAsync()
        {
            // Fetch the quote from the API
            await GetRandomQuoteAsync();

            // Simulate fetching data from your Supabase database
            CurrentPlans = GetUserLearningPlansFromDatabase();
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