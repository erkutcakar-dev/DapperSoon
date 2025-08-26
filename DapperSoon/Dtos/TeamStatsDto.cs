namespace DapperSoon.Dtos
{
    public class TeamStatsDto
    {
        public string TeamName { get; set; } = string.Empty;
        public int Goals { get; set; }
        public int GoalsConceded { get; set; }
        public float xGoals { get; set; }
        public float xGoalsConceded { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int Points { get; set; }
        public float Possession { get; set; }
    }
}
