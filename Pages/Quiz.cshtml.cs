using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Learning_site.Pages
{
    public class QuizModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public QuizModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public List<string> Topics { get; set; } = new List<string>();
        public List<QuizQuestion> CurrentQuiz { get; set; } = new List<QuizQuestion>();
        public string SelectedTopic { get; set; }

        // Use wwwroot path dynamically
        private string JsonPath => Path.Combine(_env.WebRootPath, "Quizzes", "mcq.json");

        public void OnGet(string topic)
        {
            // Load JSON file
            var json = System.IO.File.ReadAllText(JsonPath);
            var data = JsonConvert.DeserializeObject<QuizData>(json);

            Topics = data.Topics.Select(t => t.TopicName).ToList();

            if (!string.IsNullOrEmpty(topic))
            {
                SelectedTopic = topic;
                var selected = data.Topics.FirstOrDefault(t => t.TopicName == topic);
                if (selected != null)
                    CurrentQuiz = selected.MCQs;
            }
        }

        public IActionResult OnPost(string topic, List<string> answers)
        {
            var json = System.IO.File.ReadAllText(JsonPath);
            var data = JsonConvert.DeserializeObject<QuizData>(json);

            var selected = data.Topics.FirstOrDefault(t => t.TopicName == topic);
            if (selected == null)
                return Page();

            int score = 0;
            for (int i = 0; i < selected.MCQs.Count; i++)
            {
                if (answers != null && i < answers.Count && answers[i] == selected.MCQs[i].CorrectAnswer)
                    score++;
            }

            ViewData["Score"] = score;
            SelectedTopic = topic;
            CurrentQuiz = selected.MCQs;

            return Page();
        }
    }

    public class QuizData
    {
        public List<Topic> Topics { get; set; }
    }

    public class Topic
    {
        public string TopicName { get; set; }
        public List<QuizQuestion> MCQs { get; set; }
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
