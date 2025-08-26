namespace DapperSoon.Models
{
    public class Appearance
    {
        public int playerID { get; set; }
        public int gameID { get; set; }
        public int goals { get; set; }
        public int assists { get; set; }
        public bool redCard { get; set; }
        public bool yellowCard { get; set; }
        public float xGoals { get; set; }
        public float xAssists { get; set; }
        public int time { get; set; }
        public string position { get; set; } = string.Empty;
        public int positionOrder { get; set; }
        public int keyPasses { get; set; }
        public int leagueID { get; set; }
        public bool ownGoals { get; set; }
        public int shots { get; set; }
        public int substituteIn { get; set; }
        public int substituteOut { get; set; }
        public float xGoalsBuildup { get; set; }
        public float xGoalsChain { get; set; }
    }
}
