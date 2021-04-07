using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using SmisBot.Logging;


public class RedditFreeGamesOnSteam : ModuleBase<SocketCommandContext>{

    static SocketTextChannel gamesChannel;

    public DiscordSocketClient _Client;
    public RedditFreeGamesOnSteam(DiscordSocketClient _Client)
    {
        this._Client = _Client;
    }

    public void GetNew(){
            // Gets the Reddit client and channel where to send the notification
            RedditClient r = new RedditClient("bt3vamB2ZuUtow", "12447532163-GfU33RHOqQK7-haad5nOcJG0j-Gn3w", "123", "12447532163-yeStxU5OITVsfhqRVOpWSXBThCRrcg");
            gamesChannel = _Client.GetChannel(829403362029862922) as SocketTextChannel;

            //Monitors the defined subreddit
            var subreddit = r.Subreddit("FreeGamesOnSteam").About();
            subreddit.Posts.GetNew();
            subreddit.Posts.NewUpdated += C_NewPostsUpdated;
            subreddit.Posts.MonitorNew(2000, 1500);
    }   

    // When new post is detected push the notification and log it.
    public static void C_NewPostsUpdated(object sender, PostsUpdateEventArgs e)
    {   
        foreach (var post in e.Added)
        {
            Log.WriteLog("New Free Game found " + post.Author + ": " + post.Title, LogType.Log);
            PushNotification(post, gamesChannel);
        }
    } 

    // Makes an Embed notification with post data
    public static void PushNotification(Post post, SocketTextChannel textChannel){

            EmbedBuilder builder = new EmbedBuilder();
            builder.Title = post.Title;
            builder.AddField("Link: ", "http://www.reddit.com" + post.Permalink);
            builder.WithColor(Color.Green);
            builder.WithThumbnailUrl("https://media.giphy.com/media/k5PBzy5e7mLogC2XXu/giphy.gif");
            builder.WithTimestamp(System.DateTimeOffset.Now);
            
            textChannel.SendMessageAsync($"", false, builder.Build());
    }

    
}
