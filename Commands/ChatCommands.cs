using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Smisbot.Commands
{
    public class ChatCommands : ModuleBase<SocketCommandContext>
    {
		[Command("purge")]
		[Summary("Deleting messages")]
		public async Task Purge(int x)
		{
			var messagesToDelete = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, x).FlattenAsync();
			await Context.Channel.SendMessageAsync($"Deleting {x} messages.");
			await (Context.Channel as ITextChannel).DeleteMessagesAsync(messagesToDelete); 
		}

		[Command("uptime")]
		[Summary("Bot Uptime")]
		public async Task UpTime()
		{
			var timeSpan = TimeSpan.FromMilliseconds(Environment.TickCount64);
			EmbedBuilder builder = new EmbedBuilder();
			builder.WithTitle($"Time Running");
			builder.WithCurrentTimestamp();
			builder.WithThumbnailUrl("https://media.giphy.com/media/psIw9yvUL8rR3AsJwj/giphy.gif");
			builder.AddField($"Server Uptime:", $"{timeSpan:%d\\:hh\\:mm\\:ss}");
			builder.AddField($"Bot Uptime:", $"{SmisBot.Program.Uptime.getTime()}");
			builder.WithFooter($"SmisBot V0.0.1");
			await ReplyAsync($"", false, builder.Build());
		}
	}
}
