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
        private readonly string apiKey = "AIzaSyDgh-z4T0gc2EdqAPnfWqfNlA-ZFoMNisc";

        private YouTubeService GetService()
        {
            return new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "LearningSiteApp"
            });
        }

        // 🔹 Main Search: prefers playlists (structured learning), falls back to curated videos
        public async Task<List<VideoData>> SearchVideosAsync(string topic, string channelName = null, int maxResults = 50)
        {
            var youtube = GetService();

            // 1. Try to get playlist-based learning content first
            var playlistVideos = await GetPlaylistBasedVideosAsync(topic, channelName, youtube);
            if (playlistVideos.Any()) return playlistVideos;

            // 2. Otherwise, get curated individual videos
            return await GetCuratedVideosAsync(topic, channelName, youtube, maxResults);
        }

        // 🔹 Fetch Playlist Videos (structured learning)
        private async Task<List<VideoData>> GetPlaylistBasedVideosAsync(string topic, string channelName, YouTubeService youtube)
        {
            string channelId = null;
            if (!string.IsNullOrEmpty(channelName))
                channelId = await GetChannelIdByNameAsync(channelName);

            // Search for playlists
            var playlistSearch = youtube.Search.List("snippet");
            playlistSearch.Q = topic;
            playlistSearch.Type = "playlist";
            playlistSearch.MaxResults = 3;
            if (!string.IsNullOrEmpty(channelId))
                playlistSearch.ChannelId = channelId;

            var playlistResponse = await playlistSearch.ExecuteAsync();
            var playlistIds = playlistResponse.Items.Select(p => p.Id.PlaylistId).Take(1).ToList(); // take first playlist only

            if (!playlistIds.Any()) return new List<VideoData>();

            // Get videos from playlist
            var playlistItemsRequest = youtube.PlaylistItems.List("snippet,contentDetails");
            playlistItemsRequest.PlaylistId = playlistIds.First();
            playlistItemsRequest.MaxResults = 50;

            var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();
            var videoIds = playlistItemsResponse.Items.Select(v => v.ContentDetails.VideoId).ToList();

            return await FetchVideoDetailsAsync(videoIds, youtube);
        }

        // 🔹 Get individual curated videos (if no playlist found)
        private async Task<List<VideoData>> GetCuratedVideosAsync(string topic, string channelName, YouTubeService youtube, int maxResults)
        {
            var searchRequest = youtube.Search.List("snippet");
            searchRequest.Q = topic + " tutorial";
            searchRequest.MaxResults = maxResults;
            searchRequest.Type = "video";
            searchRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;

            if (!string.IsNullOrEmpty(channelName))
            {
                var channelId = await GetChannelIdByNameAsync(channelName);
                if (!string.IsNullOrEmpty(channelId))
                    searchRequest.ChannelId = channelId;
            }

            var searchResponse = await searchRequest.ExecuteAsync();
            var videoIds = searchResponse.Items
                .Where(i => i.Id.Kind == "youtube#video")
                .Select(i => i.Id.VideoId)
                .ToList();

            var videos = await FetchVideoDetailsAsync(videoIds, youtube);

            // Filter: remove Shorts (<2 mins) and irrelevant titles
            return videos
                .Where(v => GetDurationInMinutes(v.Duration) >= 2)
                .Where(v => !IsIrrelevant(v.Title))
                .OrderBy(v => v.Title) // alphabetical to create logical sequence
                .ToList();
        }

        // 🔹 Fetch video details including duration
        private async Task<List<VideoData>> FetchVideoDetailsAsync(List<string> videoIds, YouTubeService youtube)
        {
            if (!videoIds.Any()) return new List<VideoData>();

            var detailsRequest = youtube.Videos.List("contentDetails,snippet");
            detailsRequest.Id = string.Join(",", videoIds);
            var detailsResponse = await detailsRequest.ExecuteAsync();

            return detailsResponse.Items.Select(item => new VideoData
            {
                Title = item.Snippet.Title,
                Description = item.Snippet.Description,
                ChannelName = item.Snippet.ChannelTitle,
                ThumbnailUrl = item.Snippet.Thumbnails?.Medium?.Url,
                VideoUrl = $"https://www.youtube.com/watch?v={item.Id}",
                Duration = ParseYouTubeDuration(item.ContentDetails.Duration)
            }).ToList();
        }

        // 🔹 Get Channel ID from name
        private async Task<string> GetChannelIdByNameAsync(string channelName)
        {
            var youtube = GetService();
            var searchChannel = youtube.Search.List("snippet");
            searchChannel.Q = channelName;
            searchChannel.Type = "channel";
            searchChannel.MaxResults = 1;

            var response = await searchChannel.ExecuteAsync();
            return response.Items.FirstOrDefault()?.Id?.ChannelId;
        }

        // 🔹 Generate daily structured plan
        public List<DailyPlan> CreateDailyPlan(List<VideoData> videos, int days, double dailyHours)
        {
            int videosPerDay = (int)Math.Round(dailyHours * 3); // avg 20 min videos

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

        private string ParseYouTubeDuration(string isoDuration)
        {
            var timeSpan = System.Xml.XmlConvert.ToTimeSpan(isoDuration);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        private double GetDurationInMinutes(string duration)
        {
            if (TimeSpan.TryParse(duration, out var ts))
                return ts.TotalMinutes;
            return 0;
        }

        private bool IsIrrelevant(string title)
        {
            var irrelevantKeywords = new[] { "funny", "shorts", "vlog", "trailer", "prank", "memes" };
            return irrelevantKeywords.Any(k => title.ToLower().Contains(k));
        }
    }

    // DTOs
    public class VideoData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelName { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Duration { get; set; }
    }

    public class DailyPlan
    {
        public int Day { get; set; }
        public List<VideoData> Videos { get; set; }
    }
}
