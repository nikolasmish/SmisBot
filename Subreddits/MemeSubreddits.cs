using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmisBot.Logging;

namespace SmisBot.MemeSubreddits{
    public class MemeSubreddit{
        public string subredditName { get; set; }
    }
     public static class MemeList
    {
        public static List<MemeSubreddit> SubredditList = new List<MemeSubreddit>();

        public async static Task ReadFromJSON(){
            string filepath = "memeList.json";
            string jsonString = System.IO.File.ReadAllText(filepath);
            SubredditList = JsonSerializer.Deserialize<List<MemeSubreddit>>(jsonString);
            await Log.WriteLog("Finished Loading Meme Subreddit List from JSON", LogType.Log);
        }
    }

    public class JSONHandler{
        public static List<SubredditModel> subList = new List<SubredditModel>();

        public void AddToJSON(string name){
            subList.Add(new SubredditModel(){
                subredditName = name
            });
        }

        public void WriteJSON(){
            string json = JsonSerializer.Serialize(subList);
            System.IO.File.WriteAllText(@"memeList.json", json);
        }

        public void RemoveFromJSON(string name){
            if(subList.Exists(x => x.subredditName == name)){
                subList.RemoveAll(x => x.subredditName == name);
            }else {
                Log.WriteLog("That subreddit is not on the list.");
            }
        }
    }
}
