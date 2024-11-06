using Labb_3___Quiz_Configurator.Model;
using Labb_3___Quiz_Configurator.ViewModel;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace Labb_3___Quiz_Configurator.JSON
{
    class JSONQuestionImport : ViewModelBase
    {
        public JsonSerializerOptions options = new JsonSerializerOptions()
        {
            IncludeFields = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            AllowTrailingCommas = true,
        };
        private string URL;

        private string? apiKey;

        private List<(int, string)> categories = new();

        private List<OTDBcategory> LoadedCategories = new();

        private List<Questions> LoadedQuestions = new();

        public JSONDataHandling? jsonDataHandling;

        private readonly MainWindowViewModel? mainWindowViewModel;

        public JSONQuestionImport(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

        }
        public async Task<List<(int, string)>?> ImportCategories()
        {
            using (var httpClient = new HttpClient())
            {
                JsonElement json = JsonDocument.Parse(await httpClient.GetStringAsync("https://opentdb.com/api_category.php")).RootElement;
                json = json.GetProperty("trivia_categories");
                LoadedCategories = new List<OTDBcategory>(JsonSerializer.Deserialize<List<OTDBcategory>>(json.ToString()));
                foreach (var category in LoadedCategories)
                {
                    categories.Add((category.id, category.name));
                }

                return categories;
            }
        }
        public async void GetApiKey()
        {
            URL = "https://opentdb.com/api_token.php?command=request";
            using (var httpClient = new HttpClient())
            {
                JsonElement json = JsonDocument.Parse(await httpClient.GetStringAsync(URL)).RootElement;
                json = json.GetProperty("token");
                apiKey = JsonSerializer.Deserialize<string>(json, options);
            }
        }
        public async Task ImportQuestions(int numberOfQuestions, Difficulty difficultyOfQuestions, int category)
        {
            if (apiKey == null)
            {
                GetApiKey();
            }
            using (var httpClient = new HttpClient())
            {
                URL = $"https://opentdb.com/api.php?amount={numberOfQuestions}&category={category}" +
                    $"&difficulty={difficultyOfQuestions.ToString().ToLower()}&type=multiple" +
                    $"&token={apiKey}";
                JsonElement json = JsonDocument.Parse(await httpClient.GetStringAsync(URL)).RootElement;
                json = json.GetProperty("results");
                LoadedQuestions = JsonSerializer.Deserialize<List<Questions>>(json, options);
                foreach (Questions question in LoadedQuestions)
                {
                    mainWindowViewModel.ActivePack.Questions.Add(new Questions(HttpUtility.HtmlDecode(question.Question),
                        HttpUtility.HtmlDecode(question.Correct_Answer),
                        HttpUtility.HtmlDecode(question.Incorrect_Answers[0]),
                        HttpUtility.HtmlDecode(question.Incorrect_Answers[1]),
                        HttpUtility.HtmlDecode(question.Incorrect_Answers[2])));

                }

            }
        }
    }
}
