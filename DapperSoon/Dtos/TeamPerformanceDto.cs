namespace DapperSoon.Dtos
{
    public class TeamPerformanceDto
    {
        public string TeamName { get; set; } = string.Empty;
        public int Goals { get; set; }
        public int Shots { get; set; }
        public int ShotsOnTarget { get; set; }
        public int Fouls { get; set; }
        public int RedCards { get; set; }
    }
}

