using System;
using System.IO;
using System.Threading.Tasks;

namespace MusicPortalConsoleApp {
    class Program {
        private static FireBaseClient fireBaseClient;
        private static MusicPortalClient musicPortalClient;

        private static string GetPathDialog() {
            Console.Write("Enter mp3 file path: \n> ");
            string path = Console.ReadLine();
            if (path[0] == '"' && path[path.Length - 1] == '"') {
                return path.Substring(1, path.Length - 2);
            }
            return path;
        }

        private static bool ChangeTagNameDialog() {
            string answer = Console.ReadLine();
            if (answer.ToUpper().Equals("Y")) {
                return true;
            }
            return false;
        }

        private static string GetTrackNameDialog(string titleTagString) {
            if (!string.IsNullOrEmpty(titleTagString)) {
                Console.Write("Do you want to change title for '" + titleTagString + "'?\n[y/n]: ");
                if (!ChangeTagNameDialog()) {
                    return titleTagString;
                }
            }
            Console.Write("Enter track name:\n> ");
            return Console.ReadLine();
        }

        private static string GetArtistNameDialog(string artistTagString) {
            if (!string.IsNullOrEmpty(artistTagString)) {
                Console.Write("Do you want to change artist '" + artistTagString + "'?\n[y/n]: ");
                if (!ChangeTagNameDialog()) {
                    return artistTagString;
                }
            }
            Console.Write("Enter track artist name:\n> ");
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

        private static async Task StartUploadings(string path, TagLib.File file) {
            string trackName = GetTrackNameDialog(file.Tag.Title);
            string artistName = GetArtistNameDialog(file.Tag.AlbumArtists[0]);
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
            if (file == null) {
                Console.WriteLine("file not found or not supported");
                return;
            }
            fireBaseClient = new FireBaseClient();
            musicPortalClient = new MusicPortalClient();
            StartUploadings(path, file).Wait();
        }
    }
}
