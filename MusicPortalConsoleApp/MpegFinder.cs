using MusicPortalConsoleApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicPortalConsoleApp {
    public class MpegFinder {
        private List<TrackModel> audioFiles;

        public MpegFinder(string directory) {
            audioFiles = new List<TrackModel>();
            SearchForAudios(directory);
        }

        public List<TrackModel> GetAudios() {
            return audioFiles;
        }

        public void SearchForAudios(string directory) {
            List<string> files = Directory.GetFiles(directory).ToList();
            foreach (var file in files) {
                AddFileIfItsAudioWithTags(file);
            }
        }

        private void AddFileIfItsAudioWithTags(string path) {
            TagLib.File audioFile = GetTagLibFile(path);
            if (audioFile != null && HavingTags(audioFile)) {
                audioFiles.Add(GetTrackModel(audioFile, path));
            }
        }

        private TagLib.File GetTagLibFile(string path) {
            if (File.Exists(path) && path.EndsWith(".mp3")) {
                return TagLib.Mpeg.AudioFile.Create(path);
            }
            return null;
        }

        private bool HavingTags(TagLib.File file) {
            return file.Tag.Title != null && file.Tag.AlbumArtists[0] != null;
        }

        private TrackModel GetTrackModel(TagLib.File file, string path) {
            return new TrackModel {
                Name = file.Tag.Title,
                ArtistName = file.Tag.AlbumArtists[0],
                CloudURL = path
            };
        }
    }
}
