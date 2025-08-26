namespace DapperSoon.Models
{
    public class TeamStat
    {
        public int teamID { get; set; }
        public int gameID { get; set; }
        public int goals { get; set; }
        public int shots { get; set; }
        public int shotsOnTarget { get; set; }
        public float xGoals { get; set; }
        public int corners { get; set; }
        public int deep { get; set; }
        public int fouls { get; set; }
        public float ppda { get; set; }
        public int redCards { get; set; }
        public string result { get; set; } = string.Empty;
        public int season { get; set; }
        public string location { get; set; } = string.Empty;
        public DateTime date { get; set; }
        public string yellowCards { get; set; } = string.Empty;
    }
}
