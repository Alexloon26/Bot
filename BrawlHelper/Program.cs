using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using BrawlHelper.Models;
using BrawlHelper.Model;

namespace BrawlHelper
{
    class Program
    {
        private static string _token = Constants.Token;
        private static string _currentScrambledBrawler = null;
        private static string _currentBrawlerName = null;

        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient(_token);
            var brawlClient = new BrawlClient();

            var me = await botClient.GetMeAsync();

            int offset = 0;

            while (true)
            {
                var updates = await botClient.GetUpdatesAsync(offset);

                foreach (var update in updates)
                {
                    if (update.Message != null)
                    {
                        var message = update.Message;
                        var chatId = message.Chat.Id;

                        Console.WriteLine($"Received a message from {chatId}: {message.Text}");

                        try
                        {
                            if (_currentScrambledBrawler != null)
                            {
                                if (message.Text == "/exit")
                                {
                                    _currentScrambledBrawler = null;
                                    _currentBrawlerName = null;
                                    await botClient.SendTextMessageAsync(chatId, "You have exited the game.");
                                }
                                else
                                {
                                    if (message.Text.Equals(_currentBrawlerName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        await botClient.SendTextMessageAsync(chatId, "Congratulations! You guessed it right.");
                                        _currentScrambledBrawler = null;
                                        _currentBrawlerName = null;
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(chatId, "Try again! Type /exit to leave the game.");
                                    }
                                }
                            }
                            else if (message.Text == "/guessbrawlers")
                            {
                                await GetBrawlers(botClient, brawlClient, chatId);
                            }
                            else if (message.Text.StartsWith("/getinfo"))
                            {
                                var playerId = message.Text.Split(' ').ElementAtOrDefault(1);
                                if (!string.IsNullOrEmpty(playerId))
                                {
                                    await GetPlayerInfo(botClient, brawlClient, chatId, playerId);
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId, "Please provide a player ID: /getinfo {playerId}");
                                }
                            }
                            else if (message.Text.StartsWith("/compare"))
                            {
                                var playerIds = message.Text.Split(' ').Skip(1).ToArray();
                                if (playerIds.Length == 2)
                                {
                                    await ComparePlayers(botClient, brawlClient, chatId, playerIds[0], playerIds[1]);
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId, "Please provide two player IDs: /compare {playerId1} {playerId2}");
                                }
                            }
                            else if (message.Text.StartsWith("/getclubinfo"))
                            {
                                var clubId = message.Text.Split(' ').ElementAtOrDefault(1);
                                if (!string.IsNullOrEmpty(clubId))
                                {
                                    await GetClubInfo(botClient, brawlClient, chatId, clubId);
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId, "Please provide a club ID: /getclubinfo {clubId}");
                                }
                            }
                            else if (message.Text == "/getevent")
                            {
                                await GetEvents(botClient, brawlClient, chatId);
                            }
                        }
                        catch (ApiRequestException ex) when (ex.ErrorCode == 403)
                        {
                            Console.WriteLine($"Bot was blocked by the user {chatId}: {ex.Message}");
                        }
                        offset = update.Id + 1;
                    }
                }
            }
        }

        private static async Task GetBrawlers(TelegramBotClient botClient, BrawlClient brawlClient, long chatId)
        {
            var brawlers = await brawlClient.GetBrawlers();
            var random = new Random();
            var randomBrawler = brawlers.Items[random.Next(brawlers.Items.Count)];
            _currentBrawlerName = randomBrawler.Name;
            _currentScrambledBrawler = ScrambleBrawlerName(_currentBrawlerName);

            await botClient.SendTextMessageAsync(chatId, $"Guess the Brawler: {_currentScrambledBrawler}");
        }

        private static string ScrambleBrawlerName(string name)
        {
            var random = new Random();
            var nameArray = name.ToCharArray();
            for (int i = nameArray.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var t = nameArray[i];
                nameArray[i] = nameArray[j];
                nameArray[j] = t;
            }
            return new string(nameArray);
        }

        private static async Task GetPlayerInfo(TelegramBotClient botClient, BrawlClient brawlClient, long chatId, string playerId)
        {
            try
            {
                var player = await brawlClient.GetPlayerInfo(playerId);

                var playerInfo = $@"
              Player: {player.Name}
              Tag: {player.Tag}
              Trophies: {player.Trophies}
              Highest Trophies: {player.HighestTrophies}
              Experience Level: {player.ExpLevel}
              Experience Points: {player.ExpPoints}
              Solo Victories: {player.SoloVictories}
              Duo Victories: {player.DuoVictories}
              Highest Power Play Points: {player.HighestPowerPlayPoints}
              Best Robo Rumble Time: {player.BestRoboRumbleTime}
              Best Time As Big Brawler: {player.BestTimeAsBigBrawler}
              Qualified from Championship Challenge: {(player.IsQualifiedFromChampionshipChallenge ? "Yes" : "No")}
              Club: {player.Club?.Name ?? "No Club"} (Tag: {player.Club?.Tag ?? "N/A"})
              Name Color: {player.NameColor}
              Icon ID: {player.Icon?.Id ?? 0}";

                await botClient.SendTextMessageAsync(chatId, playerInfo);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Error: {ex.Message}");
            }
        }

        private static async Task ComparePlayers(TelegramBotClient botClient, BrawlClient brawlClient, long chatId, string playerId1, string playerId2)
        {
            try
            {
                var player1 = await brawlClient.GetPlayerInfo(playerId1);
                var player2 = await brawlClient.GetPlayerInfo(playerId2);

                string comparisonResult;
                if (player1.Trophies > player2.Trophies)
                {
                    comparisonResult = $"{player1.Name} ìàº á³ëüøå êóáê³â í³æ {player2.Name}";
                }
                else
                {
                    comparisonResult = $"{player2.Name} ìàº á³ëüøå êóáê³â í³æ {player1.Name}";
                }
                await botClient.SendTextMessageAsync(chatId, comparisonResult);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Error: {ex.Message}");
            }
        }

        private static async Task GetClubInfo(TelegramBotClient botClient, BrawlClient brawlClient, long chatId, string clubId)
        {
            try
            {
                var club = await brawlClient.GetClubInfo(clubId);

                var clubInfo = $@"
                Club Name: {club.Name}
                Club Tag: {club.Tag}
                Description: {club.Description}
                Trophies: {club.Trophies}
                Required Trophies: {club.RequiredTrophies}
                Members:
                {string.Join("\n", club.Members.Select(m => $"{m.Name} ({m.Tag}) - {m.Trophies} Trophies"))}";

                await botClient.SendTextMessageAsync(chatId, clubInfo);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Error: {ex.Message}");
            }
        }

        private static async Task GetEvents(TelegramBotClient botClient, BrawlClient brawlClient, long chatId)
        {
            try
            {
                var scheduledEvents = await brawlClient.GetEvents();
                var eventsInfo = string.Join("\n\n", scheduledEvents.Select(e =>
                    $"Slot ID: {e.SlotId}\n" +
                    $"Event ID: {e.Event.Id}\n" +
                    $"Mode: {e.Event.Mode}\n" +
                    $"Map: {e.Event.Map}\n" +
                    $"Start Time: {e.StartTime}\n" +
                    $"End Time: {e.EndTime}"));

                await botClient.SendTextMessageAsync(chatId, eventsInfo);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Error: {ex.Message}");
            }
        }
    }
}