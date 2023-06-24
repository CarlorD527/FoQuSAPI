using FQ.Domain.Entities;

using FQ.Infrastructure.Interfaces;

namespace FQ.Infrastructure.Repositories
{
    public class PostRepository : IFirestoreRepository<Post>
    {
      
        private readonly string CollectionName = "Posts";
        private readonly FirestoreRepository<Post> firestoreRepository;
        public PostRepository()
        {
            firestoreRepository = new FirestoreRepository<Post>(CollectionName);
        }

        public async Task<List<Post>> GetAllAsync()
        {

            return await firestoreRepository.GetAllAsync();
        }
        public async Task PostAsync(Post entity)
        {

            await firestoreRepository.PostAsync(entity);
        }

    }
}
