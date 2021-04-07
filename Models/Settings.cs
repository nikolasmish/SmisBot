using Newtonsoft.Json;

namespace SmisBot.Settings
{
    public class SettingsModel
    {
        //Json Bot Settings aww Jizzz
        [JsonProperty("token")] public string BotToken { get; set; }
        [JsonProperty("prefix")] public char Prefix { get; set; }
        [JsonProperty("task")] public int TaskSleep { get; set; }
        [JsonProperty("version")] public string Version { get; set; }
        [JsonProperty("channel")] public ulong BotChannel { get; set; }
    }

    public static class Settings
    {
        public static SettingsModel BotSettings = new SettingsModel();
    }
}
