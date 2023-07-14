using Firebase.Auth;
using Firebase.Storage;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using static Google.Rpc.Context.AttributeContext.Types;


namespace FQ.Infrastructure.FirestoreServices
{
    public class FireStoreUTILS
    {
        private const string BucketName = "tallersocialforoq.appspot.com";

        private static readonly string ApiKey = "AIzaSyDChEsw98rukHPnNs9iUZyS2ALZ3Yg-I4c ";

        private static readonly string AuthEmail = "firestoreaccount@gmail.com";

        private static readonly string AuthPassword = "x1x2x3x4x5x6";

        public FireStoreUTILS()
        {
        }

        public async Task<string> UploadImage(IFormFile formfile, string fileName)
        {

            using (var ms = new MemoryStream())
            {
                await formfile.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                BucketName,
                new FirebaseStorageOptions
                {

                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                    .Child("Fotos_Publicaciones")
                    .Child(fileName)
                    .PutAsync(ms, cancellation.Token);


                var downloadUrl = await task;

                return downloadUrl;

            }


        }
        public async Task DeleteImage(string nombreArchivo)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var cancellation = new CancellationTokenSource();

            var storage = new FirebaseStorage(
                BucketName,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                });

            await storage.Child("Fotos_Publicaciones").Child(nombreArchivo).DeleteAsync();
        }

        public async Task DeleteImageByUrl(string url) {

            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var cancellation = new CancellationTokenSource();

            Uri uri = new Uri(url);
            string nombreArchivo = Path.GetFileName(uri.LocalPath);

            var storage = new FirebaseStorage(
                BucketName,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                });

            await storage.Child("Fotos_Publicaciones").Child(nombreArchivo).DeleteAsync();

        }
    }
}
