namespace DapperSoon.Dtos
{
    public class LeagueStatsDto
    {
        public string LeagueName { get; set; } = string.Empty;
        public int TotalGames { get; set; }
        public int TotalGoals { get; set; }
        public int TotalTeams { get; set; }
        public float AverageGoalsPerGame { get; set; }
        public string Season { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}


