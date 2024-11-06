using System.Text.Json.Serialization;

namespace Labb_3___Quiz_Configurator.Model
{
    internal class Questions
    {
        public Questions(string query,
            string correctAnswer, string incorrectAnswer1,
            string incorrectAnswer2, string incorrectAnswer3)
        {
            Question = query;
            Correct_Answer = correctAnswer;
            Incorrect_Answers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }
        [JsonConstructor]
        public Questions() { }
        public string Question { get; set; }

        public string Correct_Answer { get; set; }

        public string[] Incorrect_Answers { get; set; }
    }
}
