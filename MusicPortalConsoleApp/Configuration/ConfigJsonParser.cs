using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicPortalConsoleApp.Configuration {
    public class ConfigJsonParser {
        private const string configJson = "D:\\Projects\\MusicPortalConsoleApp\\MusicPortalConsoleApp\\config.json";
        private List<string> directories;

        public ConfigJsonParser() {
            ResolveDirectories();
        }

        public List<string> GetDirectories() {
            return directories;
        }

        public void ResolveDirectories() {
            directories = ReadConfigFile();
            for (int i = 0; i < directories.Count; i++) {
                directories.AddRange(GetSubdirectories(directories[i]));
            }
        }

        private List<string> ReadConfigFile() {
            JArray models = (JArray)JsonConvert.DeserializeObject(File.ReadAllText(configJson));
            return models.Children<JObject>().Select(d => d.Properties().ElementAt(0).Value.ToString()).ToList();
        }

        private List<string> TryGetSubdirectories(string path) {
            try {
                return Directory.GetDirectories(path).ToList();
            }
            catch (UnauthorizedAccessException) {
                return new List<string>();
            }
        }

        private List<string> GetSubdirectories(string path) {
            List<string> subdirectories = TryGetSubdirectories(path);
            for (int i = 0; i < subdirectories.Count; i++) {
                subdirectories.AddRange(GetSubdirectories(subdirectories[i]));
            }

            return subdirectories;
        }
    }
}
