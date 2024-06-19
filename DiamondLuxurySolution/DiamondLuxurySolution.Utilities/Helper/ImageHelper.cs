using DiamondLuxurySolution.Utilities.Constants;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Utilities.Helper
{
    public class ImageHelper
    {
        private readonly IConfiguration _configuration;

        public ImageHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public static async Task<string> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string firebaseUrl = "";
                // Use a MemoryStream to avoid saving the file locally
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset the stream position to the beginning

                    firebaseUrl = await UploadToFirebase(memoryStream, file.FileName); // Get the download URL
                }
                return firebaseUrl;

            }
            return "";
        }


        private static async Task<string> UploadToFirebase(Stream stream, string fileName)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDf-QdQ7AYvJhbKBCjdDv_mDQa1mJEm7p8"));
            var a = await auth.SignInWithEmailAndPasswordAsync("DiamondLuxuryDeveloper@gmail.com", "Hello123@");
            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                "diamondluxuryshop-980cd.appspot.com",
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
                .Child("images")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);

            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            try
            {
                string link = await task;
                return link;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
                return null;
            }
        }
    }
}
