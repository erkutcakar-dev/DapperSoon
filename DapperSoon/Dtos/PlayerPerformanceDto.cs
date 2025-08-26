namespace DapperSoon.Dtos
{
    public class PlayerPerformanceDto
    {
        public int playerID { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public int Goals { get; set; }
        public int Assists { get; set; }
        public float xGoals { get; set; }
        public float xAssists { get; set; }
        public int GamesPlayed { get; set; }
        public int MinutesPlayed { get; set; }
        public string Position { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Nationality { get; set; } = string.Empty;
    }
}
