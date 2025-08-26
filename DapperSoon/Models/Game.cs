namespace DapperSoon.Models
{
    public class Game
    {
        public int gameID { get; set; }
        public int homeTeamID { get; set; }
        public int awayTeamID { get; set; }
        public int homeGoals { get; set; }
        public byte awayGoals { get; set; }
        public DateTime date { get; set; }
        public int leagueID { get; set; }
        public int season { get; set; }
        public int homeGoalsHalfTime { get; set; }
        public int awayGoalsHalfTime { get; set; }
        public float homeProbability { get; set; }
        public float awayProbability { get; set; }
        public float drawProbability { get; set; }
    }
}
