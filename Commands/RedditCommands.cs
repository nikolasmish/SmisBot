using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Reddit;

using SmisBot.MemeSubreddits;

namespace ForsakenNet.RedditCommands
{
    [RequireRole("Reddit")]
    public class RedditCommands : ModuleBase<SocketCommandContext>
    {
        RedditClient r = new RedditClient("bt3vamB2ZuUtow", "12447532163-GfU33RHOqQK7-haad5nOcJG0j-Gn3w", "123", "12447532163-yeStxU5OITVsfhqRVOpWSXBThCRrcg");
        JSONHandler json = new JSONHandler();

		[Command("meme")]
		[Summary("Gives out a random meme")]
		public async Task GiveRandomMeme(){
			Random rand = new Random();

            if(MemeList.SubredditList.Count == 0){
                await Context.Channel.SendMessageAsync("No subreddits added to the list! Use !memeadd followed by a subreddit name.");
                return;
            }
            var randomSubreddit = MemeList.SubredditList[rand.Next(MemeList.SubredditList.Count)];
			var randomMeme = r.Subreddit(randomSubreddit.subredditName).Posts.Hot[rand.Next(50)];
			var memeImage = randomMeme.Listing.URL;

            await Context.Channel.SendMessageAsync(memeImage);
		}

        [Command("memeadd")]
		[Summary("Adds a subreddit to the list")]
		public async Task AddSubreddit(string subredditToAdd){
            
            if(r.Subreddit(subredditToAdd).About().SubredditData.Title == null){
                await Context.Channel.SendMessageAsync("The subreddit doesn't exist. Check the name again.");
                return;
            }

            if(MemeList.SubredditList.Exists(x => x.subredditName == subredditToAdd)){
                await Context.Channel.SendMessageAsync("The subreddit is already on the list.");
                return;
            }

            var sub = new MemeSubreddit() {subredditName = subredditToAdd};
            MemeList.SubredditList.Add(sub);
            json.AddToJSON(subredditToAdd);
            json.WriteJSON();
            
			await Context.Channel.SendMessageAsync("Successfully added '" + subredditToAdd + "' to the list!");
		}

        [Command("memeremove")]
		[Summary("Removes a subreddit from the list")]
		public async Task RemoveSubreddit(string subredditToRemove){
            var sub = new MemeSubreddit() {subredditName = subredditToRemove};
            if(!MemeList.SubredditList.Exists(x => x.subredditName == subredditToRemove)){
                await Context.Channel.SendMessageAsync("That subreddit is not one the list!");
                return;
            } 
            MemeList.SubredditList.RemoveAll(x => x.subredditName == subredditToRemove);
            json.RemoveFromJSON(subredditToRemove);
            json.WriteJSON();
			await Context.Channel.SendMessageAsync("Successfully removed '" + subredditToRemove + "' from the list!");
		}

        [Command("memelist")]
        [Summary("Lists all meme subreddits")]
        public async Task ListMemeSubreddits(){
            await MemeList.ReadFromJSON();
            
            EmbedBuilder builder = new EmbedBuilder();
            builder.Title = "Subreddits: " + MemeList.SubredditList.Count;
            int counter = 1;
            foreach(MemeSubreddit subreddit in MemeList.SubredditList){
                
                builder.AddField(counter.ToString() + "." , $"r/{subreddit.subredditName}");
                counter++;
            }
            await ReplyAsync("", false, builder.Build());
        }
	}
}
