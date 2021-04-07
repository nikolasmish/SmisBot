using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SmisBot.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;


namespace SmisBot.Events
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            //Ok here the Idiot Initalized..
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;

            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            //Read stupid Text form command here and pass it through
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            //here check if it is a command and send a fucking Thumb
            var argPos = 0;
            if (!message.HasCharPrefix('!', ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);

            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            //yeah Thumb
            var thumbsUp = new Emoji("👍");

            if (!command.IsSpecified) return;

            if (result.IsSuccess)
            {
                //Log Commands..
                await Log.WriteLog($"User:{context.User} - {context.Message}", LogType.Command);
                //For my purge command no reaction needed for that..
                if (context.Message.ToString().Contains("urge")) return;
                await context.Message.AddReactionAsync(thumbsUp);
                return;
            }
        }

    }
}
