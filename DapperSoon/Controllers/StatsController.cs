using Microsoft.AspNetCore.Mvc;
using Dapper;
using DapperSoon.Context;
using DapperSoon.Models;
using DapperSoon.Dtos;

namespace DapperSoon.Controllers
{
    public class StatsController : Controller
    {
        private readonly FootballStatsDb _db;

        public StatsController(FootballStatsDb db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            using var conn = _db.GetConnection();

            // Dashboard Widgets
            var totalGames = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM games");
            var totalPlayers = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM players");
            var totalGoals = conn.ExecuteScalar<int>("SELECT SUM(goals) FROM appearances");
            var totalTeams = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM teams");
            var totalLeagues = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM leagues");
            var averageGoalsPerGame = conn.ExecuteScalar<double>("SELECT AVG(CAST(homeGoals + awayGoals AS FLOAT)) FROM games");

            var widgets = new List<DashboardWidgetDto>
            {
                new() { WidgetName = "Total Games", Value = totalGames, Icon = "fas fa-futbol", Color = "primary", Description = "Total matches played" },
                new() { WidgetName = "Total Players", Value = totalPlayers, Icon = "fas fa-users", Color = "success", Description = "Registered players" },
                new() { WidgetName = "Total Goals", Value = totalGoals, Icon = "fas fa-bullseye", Color = "warning", Description = "Goals scored" },
                new() { WidgetName = "Total Teams", Value = totalTeams, Icon = "fas fa-flag", Color = "info", Description = "Active teams" },
                new() { WidgetName = "Total Leagues", Value = totalLeagues, Icon = "fas fa-trophy", Color = "danger", Description = "Competitions" },
                new() { WidgetName = "Avg Goals/Game", Value = (int)Math.Round(averageGoalsPerGame, 1), Icon = "fas fa-chart-line", Color = "secondary", Description = "Goals per match" }
            };

            // Top Scorers
            var topScorers = conn.Query<PlayerPerformanceDto>(@"
                SELECT TOP 10 
                    p.name as PlayerName,
                    '' as TeamName,
                    SUM(a.goals) as Goals,
                    SUM(a.assists) as Assists,
                    SUM(a.xGoals) as xGoals,
                    SUM(a.xAssists) as xAssists,
                    COUNT(DISTINCT a.gameID) as GamesPlayed,
                    SUM(a.time) as MinutesPlayed,
                    a.position as Position
                FROM appearances a
                INNER JOIN players p ON a.playerID = p.playerID
                GROUP BY p.playerID, p.name, a.position
                ORDER BY SUM(a.goals) DESC
            ").ToList();

            // Team Stats
            var teamStats = conn.Query<TeamStatsDto>(@"
                SELECT TOP 10
                    t.name as TeamName,
                    SUM(ts.goals) as Goals,
                    0 as GoalsConceded,
                    SUM(ts.xGoals) as xGoals,
                    0 as xGoalsConceded,
                    COUNT(DISTINCT ts.gameID) as GamesPlayed,
                    SUM(CASE WHEN ts.result = 'W' THEN 1 ELSE 0 END) as Wins,
                    SUM(CASE WHEN ts.result = 'D' THEN 1 ELSE 0 END) as Draws,
                    SUM(CASE WHEN ts.result = 'L' THEN 1 ELSE 0 END) as Losses,
                    SUM(CASE WHEN ts.result = 'W' THEN 3 
                             WHEN ts.result = 'D' THEN 1 
                             ELSE 0 END) as Points,
                    0 as Possession
                FROM teamstats ts
                INNER JOIN teams t ON ts.teamID = t.teamID
                GROUP BY t.teamID, t.name
                ORDER BY Points DESC
            ").ToList();

            // League Stats
            var leagueStats = conn.Query<LeagueStatsDto>(@"
                SELECT 
                    l.name as LeagueName,
                    COUNT(DISTINCT g.gameID) as TotalGames,
                    SUM(g.homeGoals + g.awayGoals) as TotalGoals,
                    COUNT(DISTINCT CASE WHEN g.homeTeamID IS NOT NULL THEN g.homeTeamID END) + 
                    COUNT(DISTINCT CASE WHEN g.awayTeamID IS NOT NULL THEN g.awayTeamID END) as TotalTeams,
                    AVG(CAST(g.homeGoals + g.awayGoals AS FLOAT)) as AverageGoalsPerGame,
                    CAST(g.season AS NVARCHAR) as Season,
                    '' as Country
                FROM leagues l
                LEFT JOIN games g ON l.leagueID = g.leagueID
                GROUP BY l.leagueID, l.name, g.season
                ORDER BY TotalGames DESC
            ").ToList();

            // Chart Data for Goals vs xGoals
            var goalsVsXGoals = conn.Query<ScatterChartDataDto>(@"
                SELECT 
                    SUM(a.goals) as X,
                    SUM(a.xGoals) as Y,
                    p.name as Label,
                    'rgba(54, 162, 235, 0.6)' as Color
                FROM appearances a
                INNER JOIN players p ON a.playerID = p.playerID
                GROUP BY p.playerID, p.name
                HAVING SUM(a.goals) > 0 OR SUM(a.xGoals) > 0
                ORDER BY SUM(a.goals) DESC
            ").Take(20).ToList();

            // Monthly Goals Chart
            var monthlyGoals = conn.Query<LineChartDataDto>(@"
                SELECT 
                    FORMAT(g.date, 'yyyy-MM') as Label,
                    SUM(g.homeGoals + g.awayGoals) as Value
                FROM games g
                WHERE g.date >= DATEADD(MONTH, -12, GETDATE())
                GROUP BY FORMAT(g.date, 'yyyy-MM')
                ORDER BY Label
            ").ToList();

            var dashboardData = new
            {
                Widgets = widgets,
                TopScorers = topScorers,
                TeamStats = teamStats,
                TeamStatsTop10 = teamStats.Take(10).ToList(),
                LeagueStats = leagueStats,
                GoalsVsXGoals = goalsVsXGoals,
                MonthlyGoals = monthlyGoals,
                TopScorersChartData = topScorers.Take(10).Select(p => new { name = p.PlayerName, goals = p.Goals }).ToList()
            };

            return View(dashboardData);
        }

        public IActionResult FilteredList(string search = "", string position = "", int pageNumber = 1, int pageSize = 20)
        {
            using var conn = _db.GetConnection();

            var whereClause = "WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(search))
            {
                whereClause += " AND p.name LIKE @Search";
                parameters.Add("Search", $"%{search}%");
            }

            if (!string.IsNullOrEmpty(position))
            {
                whereClause += " AND a.position = @Position";
                parameters.Add("Position", position);
            }

            // Get total count for pagination
            var totalCount = conn.ExecuteScalar<int>($@"
                SELECT COUNT(*)
                FROM players p
                {whereClause}
            ", parameters);

            // Get paginated results
            var players = conn.Query<PlayerPerformanceDto>($@"
                SELECT 
                    p.playerID,
                    p.name as PlayerName,
                    '' as TeamName,
                    a.position as Position,
                    0 as Age,
                    '' as Nationality,
                    SUM(ISNULL(a.goals, 0)) as Goals,
                    SUM(ISNULL(a.assists, 0)) as Assists,
                    SUM(ISNULL(a.xGoals, 0)) as xGoals,
                    SUM(ISNULL(a.xAssists, 0)) as xAssists,
                    COUNT(DISTINCT a.gameID) as GamesPlayed,
                    SUM(ISNULL(a.time, 0)) as MinutesPlayed
                FROM players p
                LEFT JOIN appearances a ON p.playerID = a.playerID
                {whereClause}
                GROUP BY p.playerID, p.name, a.position
                ORDER BY p.name
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY
            ", new
            {
                Search = $"%{search}%",
                Position = position,
                Offset = (pageNumber - 1) * pageSize,
                PageSize = pageSize
            }).ToList();

            // Get unique positions for filter dropdown
            var positions = conn.Query<string>("SELECT DISTINCT position FROM appearances WHERE position IS NOT NULL ORDER BY position").ToList();

            var viewModel = new
            {
                Players = players,
                Search = search,
                Position = position,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Positions = positions
            };

            return View(viewModel);
        }
    }
}
