using Labb_3___Quiz_Configurator.Model;
using Labb_3___Quiz_Configurator.ViewModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Labb_3___Quiz_Configurator.JSON
{
    internal class JSONDataHandling
    {
        private string json;
        private readonly MainWindowViewModel? mainWindowViewModel;

        public JSONDataHandling(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public JsonSerializerOptions options = new JsonSerializerOptions()
        {
            IncludeFields = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            AllowTrailingCommas = true,
        };
        public async Task LoadQuestionPack(object obj, string? json)
        {
            if (json == null)
            {
                try
                {
                    json = await File.ReadAllTextAsync($"Default Pack.json");
                }
                catch (Exception)
                {
                    mainWindowViewModel.ActivePack = new QuestionPackViewModel(new QuestionPack("Default Pack"));
                    SaveQuestionPack(this);
                    return;
                }
            }
            var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            QuestionPack LoadedPack = await JsonSerializer.DeserializeAsync<QuestionPack>(jsonStream);
            if (mainWindowViewModel.ActivePack == null)
            {
                mainWindowViewModel.ActivePack = new QuestionPackViewModel(new QuestionPack("Default Pack"));
            }
            mainWindowViewModel.ActivePack.Name = LoadedPack.Name;
            mainWindowViewModel.ActivePack.Difficulty = LoadedPack.Difficulty;
            try
            {
                mainWindowViewModel.ActivePack.TimeLimitInSeconds = LoadedPack.TimeLimitInSeconds;
            }
            catch (Exception)
            {
                mainWindowViewModel.ActivePack.TimeLimitInSeconds = 30;
            }

            mainWindowViewModel.ActivePack.Questions.Clear();
            foreach (Question q in LoadedPack.Questions)
            {
                mainWindowViewModel.ActivePack.Questions.Add(q);
            }
            jsonStream.Close();
        }
        public async Task SaveQuestionPack(object obj)
        {
            var jsonStream = new MemoryStream();
            JsonSerializer.SerializeAsync(jsonStream, mainWindowViewModel.ActivePack, options);
            var json = Encoding.UTF8.GetString(jsonStream.ToArray());
            await File.WriteAllTextAsync($"{mainWindowViewModel.ActivePack.Name}.json", json);
            if (!mainWindowViewModel.Packs.Contains(mainWindowViewModel.ActivePack))
            {
                mainWindowViewModel.Packs.Add(mainWindowViewModel.ActivePack);
            }
            jsonStream.Close();
        }
    }
}
