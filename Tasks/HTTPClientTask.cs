using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmisBot.Tasks
{
    class HTTPClientTask
    {

        static HttpClientHandler _ClientHandler = new HttpClientHandler();

        public Util.TimerUtil taskTimer;

        public HTTPClientTask()
        {
            taskTimer = new Util.TimerUtil();
        }


        public async Task<string> getClient(string URL)
        {
            taskTimer.Reset();
            taskTimer.Start();
            using (var httpClient = new HttpClient(_ClientHandler, false))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(15);
                using(var response  = await httpClient.GetAsync(URL))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await Logging.Log.WriteLog("Bad Request", Logging.LogType.Error);
                        return null;
                    }

                    string webData = await response.Content.ReadAsStringAsync();
                    if(String.IsNullOrEmpty(webData))
                    {
                        await Logging.Log.WriteLog("Data is Null or Emtpy", Logging.LogType.Error);
                        return null;
                    } else
                    {
                        Stream webStream = await response.Content.ReadAsStreamAsync();
                        StreamReader readStream = null;

                        readStream = new StreamReader(webStream);
                        await Logging.Log.WriteLog($"HTTPClient Done in: {taskTimer.getMS()}MS", Logging.LogType.Log);
                        return readStream.ReadToEnd();
                    }
                }
                
            }
        }


    }
}
