using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SmisBot.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SmisBot.Events
{
    public class EventHandler
    {
        //Only need to Read the data no need to write info Moron
        private readonly DiscordSocketClient _Client;
        private readonly CommandService _Commands;

        public ScrapMainPage scrapMainPage;
        public RedditFreeGamesOnSteam RedditFreeGamesOnSteam;

        public EventHandler(IServiceProvider service)
        {
            //Declare the Services here..
            _Client = service.GetRequiredService<DiscordSocketClient>();
            _Commands = service.GetRequiredService<CommandService>();
            scrapMainPage = new ScrapMainPage(_Client);
            RedditFreeGamesOnSteam = new RedditFreeGamesOnSteam(_Client);

            //Events 
            _Client.Ready += ClientReady;
            _Client.UserJoined += UserJoined;
            _Client.UserLeft += UserLeft;
            _Commands.Log += Commands;
        }

        private Task Commands(LogMessage log)
        {
            Log.WriteLog($"{log.Message}", LogType.Command);
            return Task.CompletedTask;
        }

        private Task UserLeft(SocketGuildUser arg)
        {
            Log.WriteLog($"User: {arg.Nickname} left the Guild.", LogType.Log);
            return Task.CompletedTask;
        }

        private Task UserJoined(SocketGuildUser arg)
        {
            Log.WriteLog($"User: {arg.Nickname} Joined the guild.", LogType.Log);
            return Task.CompletedTask;
        }

        private async Task ClientReady()
        {
            await Log.WriteLog($"Client Ready with: {_Client.Latency}MS.", LogType.Log);
            await scrapMainPage.ScrapFrontPage();
            RedditFreeGamesOnSteam.GetNew();
            //return Task.CompletedTask;
        }
    }
}
