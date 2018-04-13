using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicPortalConsoleApp.Clients;
using MusicPortalConsoleApp.Configuration;
using MusicPortalConsoleApp.Models;

namespace MusicPortalConsoleApp {
    class Program {
        private static FireBaseClient fireBaseClient = new FireBaseClient();
        private static MusicPortalClient musicPortalClient = new MusicPortalClient();
        private static ConfigJsonParser configJsonParser = new ConfigJsonParser();
        private static TracksJsonParser tracksJsonParser = new TracksJsonParser();

        private static bool isUploaded(string trackName) {
            return tracksJsonParser.GetUploads().Select(t => t.Name).Contains(trackName);
        }

        private static async Task UploadTracks(List<TrackModel> tracks) {
            foreach (var track in tracks) {
                if (!isUploaded(track.Name)) {
                    await StartUploadings(track);
                    tracksJsonParser.AddUploaded(track);
                }
            }
        }

        private static async Task StartUploadings(TrackModel track) {
            track.CloudURL = await fireBaseClient.UploadToFirebase(track.CloudURL);
            await musicPortalClient.UploadTrack(track);
        }

        static void Main(string[] args) {
            List<string> directories = configJsonParser.GetDirectories();
            foreach (var directory in directories) {
                MpegFinder mpegFinder = new MpegFinder(directory);
                UploadTracks(mpegFinder.GetAudios()).Wait();
            }
        }
    }
}
