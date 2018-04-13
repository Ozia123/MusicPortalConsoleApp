using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MusicPortalConsoleApp {
    class MusicPortalClient {
        private Dictionary<string, string> addressDictionary;
        private HttpClient client;

        public MusicPortalClient() {
            client = new HttpClient();
            addressDictionary = new Dictionary<string, string>();
            addressDictionary.Add("1", @"http://localhost:63678/");
            addressDictionary.Add("2", @"http://172.19.1.52:45455/");
        }

        private string SelectAddressDialog() {
            Console.WriteLine("\nSelect base address for httpClient:");
            foreach (var address in addressDictionary) {
                Console.WriteLine(address.Key + ". " + address.Value);
            }
            Console.Write("> ");
            return Console.ReadLine();
        }

        private void SetClientBaseAddress() {
            string input = SelectAddressDialog();
            var dictionaryValue = addressDictionary.GetValueOrDefault(input);
            if (dictionaryValue == null) {
                dictionaryValue = addressDictionary.Values.ElementAt(0);
                Console.WriteLine("Incorrect input, using default (" + dictionaryValue + ")");
            }
            client.BaseAddress = new Uri(dictionaryValue);
        }

        public async Task UploadTrack(TrackModel model) {
            SetClientBaseAddress();
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/track/console-upload", content);
            Console.WriteLine("\nhttp.post, status: " + response.StatusCode);
            Console.WriteLine("\nheaders: " + response.Headers);
        }
    }
}
