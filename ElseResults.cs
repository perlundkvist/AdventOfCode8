using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AdventOfCode8
{
    class ElseResults
    {
        public static void Create(string filename)
        {
            var json = File.ReadAllText(filename);
            var document = JsonDocument.Parse(json);
            var codeEvent = document.RootElement.GetProperty("event").GetString();
            var year = int.Parse(codeEvent);
            var members = JsonDocument.Parse(document.RootElement.GetProperty("members").ToString());

            var players = new List<Player>();

            foreach (var member in members.RootElement.EnumerateObject())
            {
                var player = new Player { Id = member.Name, };
                players.Add(player);
                var memberData = JsonDocument.Parse(member.Value.ToString());
                player.Name = memberData.RootElement.GetProperty("name").GetString();
                player.Points = memberData.RootElement.GetProperty("local_score").GetInt32();
                var days = JsonDocument.Parse(memberData.RootElement.GetProperty("completion_day_level").ToString());
                foreach (var day in days.RootElement.EnumerateObject())
                {
                    var dayScore = new Player.DayScore { Day = int.Parse(day.Name) };
                    player.DayScores.Add(dayScore);
                    foreach (var star in JsonDocument.Parse(day.Value.ToString()).RootElement.EnumerateObject())
                    {
                        var starTime = JsonDocument.Parse(star.Value.ToString()).RootElement;
                        if (star.Name == "1")
                        {
                            dayScore.Completed1 = starTime.GetProperty("get_star_ts").GetInt64();
                            var dt = DateTimeOffset.FromUnixTimeSeconds(dayScore.Completed1).AddHours(1);      
                            dayScore.CompletedDate1 = dt.DateTime;
                            if (dayScore.CompletedDate1 > new DateTime(year, 12, 26))
                                Console.WriteLine($"{player.Name} finished part 1 on day {day.Name} {dayScore.CompletedDate1}");
                        }
                        if (star.Name == "2")
                        {
                            dayScore.Completed2 = starTime.GetProperty("get_star_ts").GetInt64();
                            var dt = DateTimeOffset.FromUnixTimeSeconds(dayScore.Completed2.Value).AddHours(1);
                            dayScore.CompletedDate2 = dt.DateTime;
                            if (dayScore.CompletedDate2 > new DateTime(year, 12, 26))
                                Console.WriteLine($"{player.Name} finished part 2 on day {day.Name} {dayScore.CompletedDate2}");
                        }
                    }
                }
            }

            var playerCount = players.Count;

            for (int day = 1; day <= 25; day++)
            {
                //Console.Write($"Dag {day}.1;Dag {day}.2;");
                if (!players.Any(p => p.DayScores.Any(d => d.Day == day)))
                    continue;

                var star1Times = players.SelectMany(p => p.DayScores.Where(d => d.Day == day).Select(d => d.Completed1)).OrderBy(t => t).ToList();
                var star2Times = players.SelectMany(p => p.DayScores.Where(d => d.Day == day && d.Completed2.HasValue).Select(d => d.Completed2)).OrderBy(t => t).ToList();
                foreach (var player in players)
                {
                    var dayScore = player.DayScores.FirstOrDefault(d => d.Day == day);
                    if (dayScore == null)
                        continue;
                    dayScore.Points1 = playerCount - star1Times.IndexOf(dayScore.Completed1);
                    if (dayScore.Completed2.HasValue)
                        dayScore.Points2 = playerCount - star2Times.IndexOf(dayScore.Completed2);
                }
            }
            Console.WriteLine();

            foreach (var player in players.OrderByDescending(p => p.Points))
            {
                Console.Write($"{player.Name};{player.Points};{player.DayScores.Count(d => d.Points1 == playerCount) + player.DayScores.Count(d => d.Points2 == playerCount)};");
                Console.Write($"{player.DayScores.Count(d => d.Points1 == playerCount - 1) + player.DayScores.Count(d => d.Points2 == playerCount - 1)};");
                Console.Write($"{player.DayScores.Count(d => d.Points1 == playerCount - 2) + player.DayScores.Count(d => d.Points2 == playerCount - 2)};");
                for (int day = 1; day <= 25; day++)
                {
                    var dayScore = player.DayScores.FirstOrDefault(d => d.Day == day);
                    Console.Write($"{dayScore?.Points1};{dayScore?.Points2};");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Uppdaterad");
            Console.WriteLine($"{DateTime.Now:g}");
        }

        internal class Player
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Points;

            public List<DayScore> DayScores { get; } = new List<DayScore>();

            public class DayScore
            {
                public int Day;
                public long Completed1;
                public long? Completed2;
                public int Points1;
                public int Points2;
                public DateTime CompletedDate1;
                public DateTime? CompletedDate2;
            }
        }
    }
}
