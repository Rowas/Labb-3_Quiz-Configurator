using Labb_3___Quiz_Configurator.Model;
using Labb_3___Quiz_Configurator.ViewModel;
using System.Net.Http;
using System.Text.Json;

namespace Labb_3___Quiz_Configurator.JSON
{
    class JSONQuestionImport : ViewModelBase
    {
        private string apiToken;
        private string URL;
        private List<string> _categories = new();

        public List<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                RaisePropertyChanged();
            }
        }

        public JSONDataHandling? jsonDataHandling;

        public async Task<List<string>?> ImportCategories()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://opentdb.com/api_category.php");
                if (response.IsSuccessStatusCode)
                {
                    string jsonStream = await response.Content.ReadAsStringAsync();
                    OTDBcategory oTDBcategory = JsonSerializer.Deserialize<OTDBcategory>(jsonStream);
                    foreach (char category in oTDBcategory.name)
                    {
                        Categories.Add(category.ToString());
                    }
                    return Categories;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<string?> ImportQuestions(int numberOfQuestions, string difficultyOfQuestions, string category)
        {
            using (var httpClient = new HttpClient())
            {
                apiToken = await httpClient.GetStringAsync("https://opentdb.com/api_token.php?command=request");
                URL = $"https://opentdb.com/api.php?amount={numberOfQuestions}&difficulty={difficultyOfQuestions}&type=multiple&category={category}&token={apiToken}";
                var response = await httpClient.GetAsync(URL);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return json;
                }
                else
                {
                    return response.ReasonPhrase;
                }
            }
        }
    }
}
