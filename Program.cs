using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SmisBot.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EventHandler = SmisBot.Events.EventHandler;

namespace SmisBot
{
    class Program
    {
        //Declare stuff
        public DiscordSocketClient _Client;
        public EventHandler eventHandler;
        public static Util.TimerUtil Uptime;
        public Tasks.StartupTask startupTask = new Tasks.StartupTask();

        static void Main(string[] args)
            => new Program().BotAsync().GetAwaiter().GetResult();


        public async Task BotAsync()
        {
            await startupTask.Startup();

            if(Settings.Settings.BotSettings.BotToken.Length <= 5)
            {
                await Logging.Log.WriteLog($"Invalid Bot Token, Check StartupTask.cs", Logging.LogType.Error);
                Console.Read();
                await Task.Delay(10000);
                return;
            }

            //Discord.Net Things
            var discordToken = Settings.Settings.BotSettings.BotToken;//Add Token or from Settings..
            var service = ConfigureServices();
            //Get Uptime as i like that 
            Uptime = new Util.TimerUtil();
            //Get DiscordSocketClient
            _Client = service.GetRequiredService<DiscordSocketClient>();
            //Handles Client Events..
            eventHandler = new EventHandler(service);

            //Bot Login, Start Bot Uptime timer
            await _Client.LoginAsync(TokenType.Bot, discordToken);
            await _Client.StartAsync();
            Uptime.Start();

            await service.GetRequiredService<CommandHandler>().InitializeAsync();
            
            // Reads Subreddits JSON for Meme commands
            await MemeSubreddits.MemeList.ReadFromJSON();

            // Loop the program forever
            await Task.Delay(Timeout.Infinite);

        }

        //Service of Discord.Net 
        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }

    }
}
