using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQ.Infrastructure.AzureService
{
    public interface IContentModeratorService 
    {
        Task<Screen> ModerateTextAsync(string text);
        Task<Evaluate> ModerateImageAsync(IFormFile imageStream);
    }
}
