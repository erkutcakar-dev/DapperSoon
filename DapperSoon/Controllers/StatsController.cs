using Microsoft.AspNetCore.Mvc;
using Dapper;
using DapperSoon.Context;
using DapperSoon.Models;
using DapperSoon.Dtos;
using System.Data;
using System.Linq;

namespace DapperSoon.Controllers
{
    public class StatsController : Controller
    {
        private readonly FootballStatsDb _db;

        public StatsController(FootballStatsDb db)
        {
            _db = db;
        }

        public IActionResult Index(int? selectedSeason = null, int pageNumber = 1, int pageSize = 10)
        {
            using var conn = _db.GetConnection();

            // Set default season to 2020 if none selected
            if (selectedSeason == null)
            {
                selectedSeason = 2020;
            }

            // Dashboard Widgets
            var totalGames = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM games");
            var totalPlayers = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM players");
            var totalGoals = conn.ExecuteScalar<int>("SELECT SUM(goals) FROM appearances");
            var totalTeams = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM teams");
            var totalLeagues = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM leagues");
            var averageGoalsPerGame = conn.ExecuteScalar<double>("SELECT AVG(CAST(homeGoals + awayGoals AS FLOAT)) FROM games");
            var totalYellowCards = conn.ExecuteScalar<int>("SELECT SUM(CASE WHEN yellowCard = 1 THEN 1 ELSE 0 END) FROM appearances");
            var totalRedCards = conn.ExecuteScalar<int>("SELECT SUM(CASE WHEN redCard = 1 THEN 1 ELSE 0 END) FROM appearances");

            var widgets = new List<DashboardWidgetDto>
            {
                new() { WidgetName = "Total Games", Value = totalGames, Icon = "fas fa-futbol", Color = "primary", Description = "Total matches played" },
                new() { WidgetName = "Total Players", Value = totalPlayers, Icon = "fas fa-users", Color = "success", Description = "Registered players" },
                new() { WidgetName = "Total Goals", Value = totalGoals, Icon = "fas fa-bullseye", Color = "warning", Description = "Goals scored" },
                new() { WidgetName = "Total Teams", Value = totalTeams, Icon = "fas fa-flag", Color = "info", Description = "Active teams" },
                new() { WidgetName = "Total Leagues", Value = totalLeagues, Icon = "fas fa-trophy", Color = "danger", Description = "Competitions" },
                new() { WidgetName = "Avg Goals/Game", Value = (int)Math.Round(averageGoalsPerGame, 1), Icon = "fas fa-chart-line", Color = "secondary", Description = "Goals per match" },
                new() { WidgetName = "Yellow Cards", Value = totalYellowCards, Icon = "fas fa-exclamation-triangle", Color = "warning", Description = "Total yellow cards" },
                new() { WidgetName = "Red Cards", Value = totalRedCards, Icon = "fas fa-times-circle", Color = "danger", Description = "Total red cards" }
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

            // Get available seasons for dropdown
            var availableSeasons = conn.Query<int>("SELECT DISTINCT season FROM games WHERE season IS NOT NULL ORDER BY season DESC").ToList();

            // League Stats with season filter
            var leagueStatsQuery = @"
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
                WHERE (@SelectedSeason IS NULL OR g.season = @SelectedSeason)
                GROUP BY l.leagueID, l.name, g.season
                ORDER BY TotalGames DESC";

            var leagueStats = conn.Query<LeagueStatsDto>(leagueStatsQuery, new { SelectedSeason = selectedSeason }).ToList();

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

            // Get total count for Team Performance pagination
            var teamPerformanceCountQuery = @"
                SELECT COUNT(DISTINCT t.teamID)
                FROM teamstats ts
                INNER JOIN teams t ON ts.teamID = t.teamID
                WHERE (@SelectedSeason IS NULL OR ts.season = @SelectedSeason)";
            
            var totalTeamPerformanceCount = conn.ExecuteScalar<int>(teamPerformanceCountQuery, new { SelectedSeason = selectedSeason });

            // Team Performance Stats with season filter and pagination
            var teamPerformanceQuery = @"
                SELECT 
                    t.name as TeamName,
                    SUM(ts.goals) as Goals,
                    SUM(ts.shots) as Shots,
                    SUM(ts.shotsOnTarget) as ShotsOnTarget,
                    SUM(ts.fouls) as Fouls,
                    SUM(ts.redCards) as RedCards
                FROM teamstats ts
                INNER JOIN teams t ON ts.teamID = t.teamID
                WHERE (@SelectedSeason IS NULL OR ts.season = @SelectedSeason)
                GROUP BY t.teamID, t.name
                ORDER BY SUM(ts.goals) DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

            var teamPerformance = conn.Query<TeamPerformanceDto>(teamPerformanceQuery, new { 
                SelectedSeason = selectedSeason,
                Offset = (pageNumber - 1) * pageSize,
                PageSize = pageSize
            }).ToList();

            var dashboardData = new
            {
                Widgets = widgets,
                TopScorers = topScorers,
                TeamStats = teamStats,
                TeamStatsTop10 = teamStats.Take(10).ToList(),
                LeagueStats = leagueStats,
                AvailableSeasons = availableSeasons,
                SelectedSeason = selectedSeason,
                GoalsVsXGoals = goalsVsXGoals,
                TeamPerformance = teamPerformance,
                TeamPerformanceChartData = GetTopRedCardsData(conn, selectedSeason),
                TopScorersChartData = topScorers.Take(10).Select(p => new { name = p.PlayerName, goals = p.Goals }).ToList(),
                // Pagination data for Team Performance
                TeamPerformancePageNumber = pageNumber,
                TeamPerformancePageSize = pageSize,
                TeamPerformanceTotalCount = totalTeamPerformanceCount,
                TeamPerformanceTotalPages = (int)Math.Ceiling((double)totalTeamPerformanceCount / pageSize)
            };

            // Check if this is an AJAX request for modal content
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TeamStatsModal", dashboardData);
            }

            return View(dashboardData);
        }

        private dynamic GetTopRedCardsData(IDbConnection conn, int? selectedSeason)
        {
            var redCardsQuery = @"
                SELECT 
                    t.name as TeamName,
                    SUM(ISNULL(ts.redCards, 0)) as RedCards,
                    SUM(ISNULL(ts.fouls, 0)) as Fouls,
                    COUNT(CASE WHEN ts.yellowCards != '' AND ts.yellowCards IS NOT NULL THEN 1 END) as YellowCards
                FROM teamstats ts
                INNER JOIN teams t ON ts.teamID = t.teamID
                WHERE (@SelectedSeason IS NULL OR ts.season = @SelectedSeason)
                GROUP BY t.teamID, t.name
                HAVING SUM(ISNULL(ts.redCards, 0)) > 0
                ORDER BY SUM(ISNULL(ts.redCards, 0)) DESC";

            var topRedCards = conn.Query(redCardsQuery, new { SelectedSeason = selectedSeason })
                                 .Take(10)
                                 .Select(t => new { 
                                     teamName = t.TeamName, 
                                     redCards = (int)t.RedCards,
                                     fouls = (int)t.Fouls,
                                     yellowCards = (int)t.YellowCards
                                 })
                                 .ToList();

            return topRedCards;
        }

        public IActionResult TeamStatistics(string search = "", string league = "", int? selectedSeason = null, int pageNumber = 1, int pageSize = 10)
        {
            using var conn = _db.GetConnection();

            // Get available seasons for dropdown
            var availableSeasons = conn.Query<int>("SELECT DISTINCT season FROM games WHERE season IS NOT NULL ORDER BY season DESC").ToList();

            // Get available leagues for dropdown
            var availableLeagues = conn.Query<string>("SELECT DISTINCT l.name FROM leagues l ORDER BY l.name").ToList();

            // Build dynamic WHERE clause for filtering
            var whereConditions = new List<string>();
            var parameters = new DynamicParameters();

            if (selectedSeason.HasValue)
            {
                whereConditions.Add("g.season = @SelectedSeason");
                parameters.Add("SelectedSeason", selectedSeason.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                whereConditions.Add("t.name LIKE @Search");
                parameters.Add("Search", $"%{search}%");
            }

            if (!string.IsNullOrEmpty(league))
            {
                whereConditions.Add("l.name = @League");
                parameters.Add("League", league);
            }

            var whereClause = whereConditions.Count > 0 ? "WHERE " + string.Join(" AND ", whereConditions) : "";

            // Get total count for Team Performance pagination
            var teamPerformanceCountQuery = $@"
                SELECT COUNT(DISTINCT t.teamID)
                FROM teamstats ts
                INNER JOIN teams t ON ts.teamID = t.teamID
                INNER JOIN games g ON ts.gameID = g.gameID
                INNER JOIN leagues l ON g.leagueID = l.leagueID
                {whereClause}";
            
            var totalTeamPerformanceCount = conn.ExecuteScalar<int>(teamPerformanceCountQuery, parameters);

            // Team Performance Stats with filters and pagination
            var teamPerformanceQuery = $@"
                SELECT 
                    t.name as TeamName,
                    SUM(ts.goals) as Goals,
                    SUM(ts.shots) as Shots,
                    SUM(ts.shotsOnTarget) as ShotsOnTarget,
                    SUM(ts.fouls) as Fouls,
                    SUM(ts.redCards) as RedCards
                FROM teamstats ts
                INNER JOIN teams t ON ts.teamID = t.teamID
                INNER JOIN games g ON ts.gameID = g.gameID
                INNER JOIN leagues l ON g.leagueID = l.leagueID
                {whereClause}
                GROUP BY t.teamID, t.name
                ORDER BY SUM(ts.goals) DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

            parameters.Add("Offset", (pageNumber - 1) * pageSize);
            parameters.Add("PageSize", pageSize);

            var teamPerformance = conn.Query<TeamPerformanceDto>(teamPerformanceQuery, parameters).ToList();

            var teamStatsData = new
            {
                TeamPerformance = teamPerformance,
                AvailableSeasons = availableSeasons,
                AvailableLeagues = availableLeagues,
                SelectedSeason = selectedSeason,
                Search = search,
                League = league,
                TeamPerformancePageNumber = pageNumber,
                TeamPerformancePageSize = pageSize,
                TeamPerformanceTotalCount = totalTeamPerformanceCount,
                TeamPerformanceTotalPages = (int)Math.Ceiling((double)totalTeamPerformanceCount / pageSize)
            };

            return View(teamStatsData);
        }

        public IActionResult FairPlayTable(string search = "", string league = "", int? selectedSeason = null, int pageNumber = 1, int pageSize = 10)
        {
            using var conn = _db.GetConnection();

            // Get available seasons for dropdown
            var availableSeasons = conn.Query<int>("SELECT DISTINCT season FROM games WHERE season IS NOT NULL ORDER BY season DESC").ToList();

            // Get available leagues for dropdown
            var availableLeagues = conn.Query<string>("SELECT DISTINCT l.name FROM leagues l ORDER BY l.name").ToList();

            // Build dynamic WHERE clause for filtering
            var whereConditions = new List<string>();
            var parameters = new DynamicParameters();

            if (selectedSeason.HasValue)
            {
                whereConditions.Add("g.season = @SelectedSeason");
                parameters.Add("SelectedSeason", selectedSeason.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                whereConditions.Add("t.name LIKE @Search");
                parameters.Add("Search", $"%{search}%");
            }

            if (!string.IsNullOrEmpty(league))
            {
                whereConditions.Add("l.name = @League");
                parameters.Add("League", league);
            }

            var whereClause = whereConditions.Count > 0 ? "WHERE " + string.Join(" AND ", whereConditions) : "";

            // Get total count for Fair Play pagination
            var fairPlayCountQuery = $@"
                SELECT COUNT(DISTINCT t.teamID)
                FROM teamstats ts
                INNER JOIN teams t ON ts.teamID = t.teamID
                INNER JOIN games g ON ts.gameID = g.gameID
                INNER JOIN leagues l ON g.leagueID = l.leagueID
                {whereClause}";
            
            var totalFairPlayCount = conn.ExecuteScalar<int>(fairPlayCountQuery, parameters);

            // Fair Play Stats with filters and pagination
            parameters.Add("Offset", (pageNumber - 1) * pageSize);
            parameters.Add("PageSize", pageSize);

            var fairPlayQuery = $@"
                SELECT 
                    t.name as TeamName,
                    SUM(ISNULL(ts.fouls, 0)) as Fouls,
                    SUM(ISNULL(ts.redCards, 0)) as RedCards,
                    COUNT(CASE WHEN ts.yellowCards != '' AND ts.yellowCards IS NOT NULL THEN 1 END) as YellowCards,
                    l.name as LeagueName
                FROM teamstats ts
                INNER JOIN teams t ON ts.teamID = t.teamID
                INNER JOIN games g ON ts.gameID = g.gameID
                INNER JOIN leagues l ON g.leagueID = l.leagueID
                {whereClause}
                GROUP BY t.teamID, t.name, l.name
                ORDER BY SUM(ISNULL(ts.fouls, 0)) DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

            var fairPlayData = conn.Query(fairPlayQuery, parameters)
                                  .Select(fp => new {
                                      TeamName = fp.TeamName,
                                      Fouls = (int)fp.Fouls,
                                      RedCards = (int)fp.RedCards,
                                      YellowCards = (int)fp.YellowCards,
                                      LeagueName = fp.LeagueName
                                  }).ToList();

            var fairPlayTableData = new
            {
                FairPlayData = fairPlayData,
                AvailableSeasons = availableSeasons,
                AvailableLeagues = availableLeagues,
                Search = search,
                League = league,
                SelectedSeason = selectedSeason,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalFairPlayCount,
                TotalPages = (int)Math.Ceiling((double)totalFairPlayCount / pageSize)
            };

            return View(fairPlayTableData);
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

            // Position filter will be applied later in the CTE result
            parameters.Add("Position", position);

            // Get total count for pagination (distinct players only)
            var totalCountQuery = $@"
                WITH PlayerStats AS (
                    SELECT DISTINCT
                        p.playerID,
                        (SELECT TOP 1 a2.position 
                         FROM appearances a2 
                         WHERE a2.playerID = p.playerID 
                         GROUP BY a2.position 
                         ORDER BY SUM(a2.time) DESC) as Position
                    FROM players p
                    LEFT JOIN appearances a ON p.playerID = a.playerID
                    {whereClause}
                )
                SELECT COUNT(*) FROM PlayerStats
                {(string.IsNullOrEmpty(position) ? "" : "WHERE Position = @Position")}";
            
            var totalCount = conn.ExecuteScalar<int>(totalCountQuery, parameters);

            // Get paginated results with primary position (most played)
            var players = conn.Query<PlayerPerformanceDto>($@"
                WITH PlayerStats AS (
                    SELECT 
                        p.playerID,
                        p.name as PlayerName,
                        '' as TeamName,
                        -- Get the position where player played the most minutes
                        (SELECT TOP 1 a2.position 
                         FROM appearances a2 
                         WHERE a2.playerID = p.playerID 
                         GROUP BY a2.position 
                         ORDER BY SUM(a2.time) DESC) as Position,
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
                    GROUP BY p.playerID, p.name
                )
                SELECT * FROM PlayerStats
                {(string.IsNullOrEmpty(position) ? "" : "WHERE Position = @Position")}
                ORDER BY PlayerName
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
