﻿using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace FQ.Infrastructure.Repositories
{
    public class FirestoreRepository<T> where T: class
    {
        private readonly string CollectionName;

        public FirestoreDb firestoreDb;

        public FirestoreRepository(string collectionName)
        {
            string projectPath = Directory.GetCurrentDirectory();
            string credentialsPath = Path.Combine(projectPath, "tallersocialforoq-firebase-adminsdk-i5x2h-f96267696b.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
            firestoreDb = FirestoreDb.Create("" +
                "tallersocialforoq");
            this.CollectionName= collectionName;

        }

        public async Task<List<T>> GetAllAsync()
        {
            CollectionReference collectionRef = firestoreDb.Collection(CollectionName);
            QuerySnapshot snapshot = await collectionRef.GetSnapshotAsync();

            List<T> resultList = new List<T>();

            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                T document = documentSnapshot.ConvertTo<T>();
                resultList.Add(document);
            }

            return resultList;
        }
        public async Task PostAsync(T entity)
        {
            CollectionReference collRef = firestoreDb.Collection(CollectionName);
            await collRef.AddAsync(entity);
        }

        public async Task<List<T>> GetByIdAsync(string id)
        {
            DocumentReference documentRef = firestoreDb.Collection(CollectionName).Document(id);
            DocumentSnapshot snapshot = await documentRef.GetSnapshotAsync();

            List<T> resultList = new List<T>();

            if (snapshot.Exists)
            {
                T document = snapshot.ConvertTo<T>();
                resultList.Add(document);

                return resultList;
            }
            else {
                return null;
            }
           
        }
        public async Task<bool> DeleteByIdAsync(string id)
        {
            DocumentReference documentRef = firestoreDb.Collection(CollectionName).Document(id);
            await documentRef.DeleteAsync();

            // Verificar si el documento fue eliminado correctamente
            DocumentSnapshot snapshot = await documentRef.GetSnapshotAsync();
            return !snapshot.Exists;
        }
    }
}
