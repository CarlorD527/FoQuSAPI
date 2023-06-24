using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQ.Application.Dtos.Posts
{
   public class AddPostDto
    {
        [FirestoreProperty]
        public string? Titulo { get; set; }

        public IFormFile? Imagen { get; set; }

        [FirestoreProperty]
        public string? Contenido { get; set; }
        [FirestoreProperty]
        public string? Categoria { get; set; }
 
    }
}
