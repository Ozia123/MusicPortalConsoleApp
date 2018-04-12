using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortalConsoleApp {
    class MusicPortalClient {
        private HttpClient client;

        public MusicPortalClient() {
            client = new HttpClient();
            client.BaseAddress = new Uri(@"http://localhost:63678/");
        }

        public async Task UploadTrack(TrackModel model) {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/track/console-upload", content);
            Console.WriteLine("\n\nhttp.post, status: " + response.StatusCode);
            Console.WriteLine("\n\nheaders: " + response.Headers);
            Console.WriteLine("\n\ncontent: " + response.Content);
        }
    }
}
