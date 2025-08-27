using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_site.Pages
{
    public class YouTubePlanService
    {
        private readonly string apiKey = "AIzaSyDgh-z4T0gc2EdqAPnfWqfNlA-ZFoMNisc"; //  YouTube API key

        private YouTubeService GetService()
        {
            return new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "LearningSiteApp"
            });
        }

        // 🔹 Search for videos (if channel provided, try to restrict search)
        public async Task<List<VideoData>> SearchVideosAsync(string topic, string? channelName = null, int maxResults = 50)
        {
            var youtube = GetService();
            var searchRequest = youtube.Search.List("snippet");
            searchRequest.Q = topic;
            searchRequest.MaxResults = maxResults;
            searchRequest.Type = "video";

            if (!string.IsNullOrEmpty(channelName))
            {
                var channelId = await GetChannelIdByNameAsync(channelName);
                if (!string.IsNullOrEmpty(channelId))
                {
                    searchRequest.ChannelId = channelId;
                }
            }

            var searchResponse = await searchRequest.ExecuteAsync();

            var videoIds = searchResponse.Items
                .Where(i => i.Id.Kind == "youtube#video")
                .Select(i => i.Id.VideoId)
                .ToList();

            // Fetch durations
            var videoDetailsRequest = youtube.Videos.List("contentDetails,snippet");
            videoDetailsRequest.Id = string.Join(",", videoIds);
            var videoDetailsResponse = await videoDetailsRequest.ExecuteAsync();

            var videos = new List<VideoData>();
            foreach (var item in videoDetailsResponse.Items)
            {
                videos.Add(new VideoData
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ChannelName = item.Snippet.ChannelTitle,
                    ThumbnailUrl = item.Snippet.Thumbnails?.Medium?.Url ?? "",
                    VideoUrl = $"https://www.youtube.com/watch?v={item.Id}",
                    Duration = ParseYouTubeDuration(item.ContentDetails.Duration)
                });
            }

            return videos;
        }

        // 🔹 Get Channel ID from channel name
        private async Task<string?> GetChannelIdByNameAsync(string channelName)
        {
            var youtube = GetService();
            var searchChannel = youtube.Search.List("snippet");
            searchChannel.Q = channelName;
            searchChannel.Type = "channel";
            searchChannel.MaxResults = 1;

            var response = await searchChannel.ExecuteAsync();
            return response.Items.FirstOrDefault()?.Id?.ChannelId;
        }

        // 🔹 Create daily plan from video list
        public List<DailyPlan> CreateDailyPlan(List<VideoData> videos, int days, double dailyHours)
        {
            // Assumption: avg video length ~ 15 mins = 4 videos/hour
            int videosPerDay = (int)Math.Round(dailyHours * 4);

            var dailyPlans = new List<DailyPlan>();
            for (int i = 0; i < days; i++)
            {
                var dayVideos = videos.Skip(i * videosPerDay).Take(videosPerDay).ToList();
                if (dayVideos.Any())
                {
                    dailyPlans.Add(new DailyPlan
                    {
                        Day = i + 1,
                        Videos = dayVideos
                    });
                }
            }
            return dailyPlans;
        }

        // Helper to parse ISO 8601 duration to readable format
        private string ParseYouTubeDuration(string isoDuration)
        {
            var timeSpan = System.Xml.XmlConvert.ToTimeSpan(isoDuration);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }

    // DTOs
    public class VideoData
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ChannelName { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;  // Added property for video length
    }

    public class DailyPlan
    {
        public int Day { get; set; }
        public List<VideoData> Videos { get; set; } = new List<VideoData>();
    }
}
