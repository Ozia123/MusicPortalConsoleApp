using System;
using System.IO;
using System.Threading.Tasks;

namespace MusicPortalConsoleApp {
    class Program {
        private static FireBaseClient fireBaseClient;
        private static MusicPortalClient musicPortalClient;

        private static string GetPathDialog() {
            Console.Write("Enter mp3 file path: \n> ");
            return Console.ReadLine();
        }

        private static TagLib.File GetTagLibFile(string path) {
            if (File.Exists(path) && path.EndsWith(".mp3")) {
                return TagLib.Mpeg.AudioFile.Create(path);
            }
            return null;
        }

        private static bool HavingTags(TagLib.File file) {
            return file.Tag.Title != null && file.Tag.AlbumArtists[0] != null;
        }

        private static async Task StartUploadings(string path, string trackName, string artistName) {
            string cloudURL = await fireBaseClient.UploadToFirebase(path);
            TrackModel track = GetTrackModel(trackName, artistName, cloudURL);
            await musicPortalClient.UploadTrack(track);
        }

        private static TrackModel GetTrackModel(string trackName, string artistName, string cloudURL) {
            return new TrackModel {
                Name = trackName,
                ArtistName = artistName,
                CloudURL = cloudURL
            };
        }

        static void Main(string[] args) {
            string path = GetPathDialog();
            TagLib.File file = GetTagLibFile(path);
            if (file == null && HavingTags(file)) {
                Console.WriteLine("file not found or not supported");
                return;
            }
            fireBaseClient = new FireBaseClient();
            musicPortalClient = new MusicPortalClient();
            StartUploadings(path, file.Tag.Title, file.Tag.AlbumArtists[0]).Wait();
        }
    }
}
