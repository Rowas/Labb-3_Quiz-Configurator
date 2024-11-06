namespace Labb_3___Quiz_Configurator.Model
{
    enum Difficulty { Easy, Medium, Hard }
    internal class QuestionPack
    {
        public QuestionPack(string name,
                            Difficulty difficulty = Difficulty.Medium,
                            int timeLimitInSeconds = 30)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = new List<Questions>();
        }

        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public int TimeLimitInSeconds { get; set; }
        public List<Questions> Questions { get; set; }
    }
}
