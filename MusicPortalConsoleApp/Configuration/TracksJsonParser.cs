using MusicPortalConsoleApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MusicPortalConsoleApp.Configuration {
    class TracksJsonParser {
        private const string tracksJson = "tracks.json";

        private List<TrackModel> tracks;

        public TracksJsonParser() {
            tracks = new List<TrackModel>();
            RefreshUploads();
        }

        public void AddUploaded(TrackModel track) {
            tracks.Add(track);
            string json = SerializeTracks();
            WriteFile(json);
        }

        public void AddUploadedRange(List<TrackModel> uploadedTracks) {
            tracks.AddRange(uploadedTracks);
            string json = SerializeTracks();
            WriteFile(json);
        }

        public void RefreshUploads() {
            string json = ReadFile();
            tracks = JsonConvert.DeserializeObject<List<TrackModel>>(json) ?? new List<TrackModel>();
        }

        public List<TrackModel> GetUploads() {
            return tracks;
        }

        private string ReadFile() {
            if (!File.Exists(tracksJson)) {
                File.Create(tracksJson);
                return "";
            }
            return File.ReadAllText(tracksJson);
        }

        private void WriteFile(string json) {
            if (File.Exists(tracksJson)) {
                File.WriteAllText(tracksJson, json);
            }
        }

        private string SerializeTracks() {
            return JsonConvert.SerializeObject(tracks, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });
        }
    }
}
