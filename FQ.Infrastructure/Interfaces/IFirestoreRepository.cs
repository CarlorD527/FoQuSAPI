using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQ.Infrastructure.Interfaces
{
    public interface IFirestoreRepository<T>
    {

        Task<List<T>> GetAllAsync();
        Task PostAsync(T entity);

        Task<bool> DeleteByIdAsync(string id);
    }
}
