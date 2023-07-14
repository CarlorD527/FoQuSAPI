using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQ.Domain.Entities
{
    public class FirebaseDocument
    {
        [FirestoreDocumentId]
        public string ?Id { get; set; }
    }
}
