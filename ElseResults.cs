using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace AdventOfCode8
{
    class ElseResults
    {
        public static void Create(string json)
        {
            var document = JsonDocument.Parse(json);
            var codeEvent = document.RootElement.GetProperty("event").GetString();
            ArgumentNullException.ThrowIfNull(codeEvent);
            var year = int.Parse(codeEvent);
            var members = JsonDocument.Parse(document.RootElement.GetProperty("members").ToString());

            var players = new List<Player>();

            foreach (var member in members.RootElement.EnumerateObject())
            {
                var memberData = JsonDocument.Parse(member.Value.ToString());
                var player = new Player(member.Name ?? "Okänd",
                    memberData?.RootElement.GetProperty("name").GetString() ?? "Okänd",
                    memberData?.RootElement.GetProperty("local_score").GetInt32() ?? 0
                );
                players.Add(player);
                if (memberData == null)
                    continue;
                var days = JsonDocument.Parse(memberData.RootElement.GetProperty("completion_day_level").ToString());
                if (days == null)
                    continue;
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

            //Clipboard.SetData(DataFormats.Text, (Object)textData);
        }

        internal record Player(string Id, string Name, int Points)
        {
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

        public static string GetTopList()
        {
            try
            {
                var cookieValue = "53616c7465645f5f18af7d51b4dc80ac0c22d2311ab298fc349f11d90bf592a3f6622eb9f9ca97898e90db864627b285f327f2e284c62182a7da7cd7f71e7cee";
                var cookieContainer = new CookieContainer();
                var cookie = new Cookie("session", cookieValue) { Domain = ".adventofcode.com" };
                cookieContainer.Add(cookie);
                using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
                using var client = new HttpClient(handler);
                var response = client.GetAsync("https://adventofcode.com/2023/leaderboard/private/view/1013538.json").Result;
                if (response != null && response.IsSuccessStatusCode)
                    return response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(response?.ToString() ?? "Null response");
                return "";
            }
            catch (Exception err) 
            {
                Console.WriteLine(err.ToString());
                return ""; 
            }

        }
    }
}
