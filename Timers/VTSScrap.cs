using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using SmisBot.Logging;

public class ScrapMainPage : ModuleBase<SocketCommandContext>{

    DiscordSocketClient _Client;
    public ScrapMainPage(DiscordSocketClient _Client)
    {
        this._Client = _Client;
    }

    string oldTitle = "";
    string oldDescripton = "";

    public async Task ScrapFrontPage(){
        while(true){
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://vtsnis.edu.rs/");

            // Scraps the site for data
            var title = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[1]/div[1]/div/div/div[2]/div[1]/div[2]/h3").InnerText;
            var description = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[1]/div[1]/div/div/div[2]/div[1]/div[2]/div").InnerText;
            var date = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[1]/div[1]/div/div/div[2]/div[1]/div[1]/div").InnerText;

            // Sets the client channel where to post updates.
            var VTSChannel = _Client.GetChannel(829384592896426035) as SocketTextChannel;

            // Checks if there is update
            if(title != oldTitle && description != oldDescripton){
                
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle(title);
                builder.AddField("Date: ", date);
                builder.AddField("Description: ", description);
                await VTSChannel.SendMessageAsync($"", false, builder.Build()); 
            }else{
                await Log.WriteLog("No updates at this time at https://vtsnis.edu.rs/", LogType.Log);
            }
            
            // Sets old titles so it doesn't post same 'updates'
            oldTitle = title;
            oldDescripton = description;

            await Task.Delay(200000);
        }
    }    
}
