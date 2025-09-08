using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Learning_site.Pages
{
    public class ChatBotModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string YT_API_KEY = "AIzaSyDgh-z4T0gc2EdqAPnfWqfNlA-ZFoMNisc"; //  YouTube API key

        public ChatBotModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string Prompt { get; set; }

        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(Prompt))
            {
                Messages.Add(new ChatMessage { Role = "user", Text = Prompt });

         
                string processedPrompt = ProcessPrompt(Prompt);

                var aiResponse = await AskGeminiAsync(processedPrompt);
                string formatted = aiResponse.Replace("\n", "<br/>");


                var ytVideos = await GetYouTubeVideosAsync(processedPrompt);

                Messages.Add(new ChatMessage
                {
                    Role = "bot",
                    Text = aiResponse,
                    TextFormatted = formatted,
                    YouTubeVideos = ytVideos
                });
            }

            return Page();
        }

        private string ProcessPrompt(string prompt)
        {

            string pattern = @"\b(tell me about|explain|what is|give me info on)\b";
            string processed = Regex.Replace(prompt.ToLower(), pattern, "", RegexOptions.IgnoreCase).Trim();
            return string.IsNullOrEmpty(processed) ? prompt : processed;
        }

        private async Task<string> AskGeminiAsync(string prompt)
        {
            const string GEMINI_API_KEY = "AIzaSyCE58-h4E5Sp2GijN4XnLN4DkKDVquzpOo";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[] { new { text = $"You are an educational AI assistant. Explain clearly in professional, simple paragraphs. Question: {prompt}" } }
                    }
                }
            };

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent"
            );

            request.Headers.Add("x-goog-api-key", GEMINI_API_KEY);
            request.Content = JsonContent.Create(payload);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var parts = candidates[0].GetProperty("content").GetProperty("parts");
                    var sb = new StringBuilder();
                    foreach (var p in parts.EnumerateArray())
                    {
                        if (p.TryGetProperty("text", out var t) && t.ValueKind == JsonValueKind.String)
                            sb.AppendLine(t.GetString());
                    }
                    return sb.ToString();
                }
                return "No response generated.";
            }
            catch
            {
                return "Error parsing response: " + json;
            }
        }

        private async Task<List<YouTubeVideo>> GetYouTubeVideosAsync(string query)
        {
            var videos = new List<YouTubeVideo>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={System.Net.WebUtility.UrlEncode(query)}&key={YT_API_KEY}&type=video&maxResults=3";
                var response = await client.GetFromJsonAsync<YouTubeApiResponse>(url);

                if (response?.Items != null)
                {
                    foreach (var item in response.Items)
                    {
                        videos.Add(new YouTubeVideo
                        {
                            Title = item.Snippet.Title,
                            Url = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                            Thumbnail = item.Snippet.Thumbnails.Medium.Url
                        });
                    }
                }
            }
            catch { }

            return videos;
        }

        // YouTube API response models
        public class YouTubeApiResponse
        {
            public List<YouTubeItem> Items { get; set; }
        }

        public class YouTubeItem
        {
            public YouTubeId Id { get; set; }
            public YouTubeSnippet Snippet { get; set; }
        }

        public class YouTubeId { public string VideoId { get; set; } }
        public class YouTubeSnippet
        {
            public string Title { get; set; }
            public YouTubeThumbnails Thumbnails { get; set; }
        }

        public class YouTubeThumbnails { public YouTubeThumbnail Medium { get; set; } }
        public class YouTubeThumbnail { public string Url { get; set; } }
    }

    public class ChatMessage
    {
        public string Role { get; set; } 
        public string Text { get; set; }
        public string TextFormatted { get; set; }
        public List<YouTubeVideo> YouTubeVideos { get; set; } = new List<YouTubeVideo>();
    }

    public class YouTubeVideo
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
    }
}
