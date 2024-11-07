using Labb_3___Quiz_Configurator.Dialogs;
using Labb_3___Quiz_Configurator.Model;
using Labb_3___Quiz_Configurator.ViewModel;
using System.Net.Http;
using System.Text.Json;
using System.Web;
using System.Windows;

namespace Labb_3___Quiz_Configurator.JSON
{
    internal class JSONQuestionImport : ViewModelBase
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

        private bool _importStatus = false;

        private string? apiKey;

        private List<(int, string)> categories = new();

        private List<OTDBcategory> LoadedCategories = new();

        private List<Questions> LoadedQuestions = new();

        private JSONDataHandling? jsonDataHandling;
        private MainWindowViewModel? mainWindowViewModel;
        private QuestionImportDialog? questionImportDialog;
        private ConfigurationViewModel? configurationViewModel;

        public bool ImportStatus
        {
            get => _importStatus;
            set
            {
                _importStatus = value;
                RaisePropertyChanged();
            }
        }

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
            try
            {
                string id = "";
                string name = "";

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
                    JsonElement jsonResult = json.GetProperty("response_code");
                    if (jsonResult.GetInt32() != 0)
                    {
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Error;
                        switch (json.GetInt32())
                        {
                            case 1:
                                id = "No Results";
                                name = "Could not return results. The API doesn't have enough questions for your query.";

                                return;
                            case 2:
                                id = "Invalid Parameter";
                                name = "Your request contained an invalid parameter.";

                                return;
                            case 3:
                                id = "Token Not Found";
                                name = "Session Token does not exist.";

                                return;
                            case 4:
                                id = "Token Empty";
                                name = "Session Token has returned all possible questions for the specified query. Resetting the Token is necessary.";

                                return;
                            case 5:
                                id = "Rate Limit Exceeded";
                                name = "Request has exceeded the maximum amount of requests allowed.";

                                return;
                            default:
                                break;
                        }
                        MessageBox.Show(name, id, button, icon, MessageBoxResult.OK);
                    }
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
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Too Many Requests, Try again in a moment!", "Warning");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import Failed, Try again in a moment!", "Warning");
            }
        }
    }
}
