using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MusicPortalConsoleApp.Models;

namespace MusicPortalConsoleApp.Clients {
    class MusicPortalClient {
        private Dictionary<string, string> addressDictionary;
        private HttpClient client;

        public MusicPortalClient() {
            client = new HttpClient();
            addressDictionary = new Dictionary<string, string>();
            addressDictionary.Add("1", @"http://localhost:63678/");
            addressDictionary.Add("2", @"http://172.19.1.52:45455/");
            SetClientBaseAddress();
        }

        private void SetClientBaseAddress() {
            client.BaseAddress = new Uri(addressDictionary.Values.ElementAt(1));
        }

        public async Task UploadTrack(TrackModel model) {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/track/console-upload", content);
            Console.WriteLine("uploaded '" + model.ArtistName + " - " + model.Name + "', status: " + response.StatusCode);
        }
    }
}
