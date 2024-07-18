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
        private static readonly string _firebaseAuthApiKey;
        private static readonly string _firebaseEmail;
        private static readonly string _firebasePassword;
        private static readonly string _firebaseBucket;
        static ImageHelper()
        {
            // Calculate the correct relative path to the configuration file
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var relativePath = @"..\..\..\appsettings.json";
            var configPath = Path.GetFullPath(Path.Combine(basePath, relativePath));

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Configuration file not found: {configPath}");
            }

            // Load the configuration
            var builder = new ConfigurationBuilder()
                .AddJsonFile(configPath, optional: false, reloadOnChange: true);

            var config = builder.Build();


            _firebaseAuthApiKey = config["Settings:FirebaseSettings:AuthApiKey"];
            _firebaseEmail = config["Settings:FirebaseSettings:Email"];
            _firebasePassword = config["Settings:FirebaseSettings:Password"];
            _firebaseBucket = config["Settings:FirebaseSettings:Bucket"];
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
			var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseAuthApiKey));
			var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseEmail, _firebasePassword);
			var cancellation = new CancellationTokenSource();

			var task = new FirebaseStorage(
				_firebaseBucket,
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
