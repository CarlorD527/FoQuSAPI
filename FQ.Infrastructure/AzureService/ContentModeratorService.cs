using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System.Text;
using System.IO;

namespace FQ.Infrastructure.AzureService
{
    public class ContentModeratorService : IContentModeratorService
    {
        private readonly ContentModeratorClient _client;

        public  ContentModeratorService ()
        {
            string subscriptionKey = "056100883cf44660ba8b47edc669b44f";
            string endpoint = "https://moderadorcontenido527.cognitiveservices.azure.com/";

            _client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(subscriptionKey));
            _client.Endpoint = endpoint;
        }

        public async Task<Evaluate> ModerateImageAsync(IFormFile imageStream)
        {
            // Convertir la imagen a bytes
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }
            // Llamar al servicio de moderación de imagen
            using (var imageMemoryStream = new MemoryStream(imageData))
            {
                var result = await _client.ImageModeration.EvaluateFileInputAsync(imageMemoryStream, true);

                return result;
            }
        }

        public Task<Screen> ModerateTextAsync(string text)
        {
            throw new NotImplementedException();
        }

        //// INTENTO DE IMPLEMENTACION 
        //public async Task<Screen> ModerateTextAsync(string text)
        //{
        //    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
        //    {
        //        var result = await _client.TextModeration.ScreenTextAsync("text/plain", stream,"spa",false,false,null,true,default);

        //        return result;

        //    }
        //}
    }
}
