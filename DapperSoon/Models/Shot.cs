namespace DapperSoon.Models
{
    public class Shot
    {
        public int gameID { get; set; }
        public int shooterID { get; set; }
        public int assisterID { get; set; }
        public float positionX { get; set; }
        public float positionY { get; set; }
        public string shotResult { get; set; } = string.Empty;
        public float xGoal { get; set; }
        public string shotType { get; set; } = string.Empty;
        public string situation { get; set; } = string.Empty;
        public int minute { get; set; }
        public string lastAction { get; set; } = string.Empty;
    }
}
