using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;

namespace FQ.Domain.Entities
{
    [FirestoreData]
    public class Post : FirebaseDocument
    {

        [FirestoreProperty]
        public string? Titulo { get; set; }

        [FirestoreProperty]
        public string? Contenido { get; set; }
        [FirestoreProperty]
        public string? Categoria { get; set; }
        [FirestoreProperty]
        public string? UrlImagen { get; set; }
        [FirestoreProperty]
        public DateTime? fechaCreacion { get; set; }
        [FirestoreProperty]
        public DateTime? fechaActualizacion{ get; set; }
    }
}
