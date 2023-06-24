using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Rpc.Context.AttributeContext.Types;
using static System.Net.Mime.MediaTypeNames;

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



        public Task<Dictionary<string, string>> ModerateTextAsync(string text)
        {
            throw new NotImplementedException();
        }

        // INTENTO DE IMPLEMENTACION 
        //public async Task<Dictionary<string,string>> ModerateTextAsync (string text)
        //{
        //    var result = await _client.TextModeration.ScreenTextAsync("text/plain", new MemoryStream(Encoding.UTF8.GetBytes(text)));

        //    if (result != null && result.Classification != null && result.OriginalText != null) {

        //        var moderationResult = new Dictionary<string, string>
        //        {
        //             { "Resultado", result.Classification.ReviewRecommended!.Value.ToString() },
        //             { "TextoOriginal", result.OriginalText.ToString() },
        //        };

        //        return  moderationResult;
        //    }

        //      else
        //    {
        //        var dic = new Dictionary<string, string>
        //        {
        //            { "resultado", "no se clasifico o no se encontro nada!?" }
        //        };
        //        return dic;

        //    }
        //}
    }
}
