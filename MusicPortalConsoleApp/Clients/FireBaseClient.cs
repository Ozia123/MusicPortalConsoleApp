using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Firebase.Auth;

namespace MusicPortalConsoleApp.Clients {
    class FireBaseClient {
        private static Random rand = new Random();
        private readonly string apiKey = @"AIzaSyAKL84VNKWSiihosvSTD1z6vc44lAeAhWI";
        private readonly string bucket = @"musicportal-cecd8.appspot.com";
        private FirebaseAuthProvider auth;
        private FirebaseAuthLink authLink;

        public FireBaseClient() {
            auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            SignIn().Wait();
        }

        private async Task SignIn() {
            authLink = await auth.SignInAnonymouslyAsync();
        }

        public async Task<string> UploadToFirebase(string path) {
            var stream = File.Open(path, FileMode.Open);
            string fileName = path.Substring(path.LastIndexOf("\\") + 1);
            var task = new FirebaseStorage(bucket,
                new FirebaseStorageOptions {
                    AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken),
                    ThrowOnCancel = false
                })
                .Child("console-tracks")
                .Child(rand.Next().ToString() + fileName)
                .PutAsync(stream);

            return await task;
        }
    }
}
