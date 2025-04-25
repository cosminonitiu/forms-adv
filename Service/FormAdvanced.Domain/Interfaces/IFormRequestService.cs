using FormAdvanced.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Interfaces
{
    public interface IFormRequestService
    {
        Task<FormRequest?> GetByIdAsync(string id, string owner);
        Task<List<FormRequest>> GetByOwnerAsync(string owner);
        Task<List<FormRequest>> GetAllAsync();
        Task<FormRequest> UpsertAsync(FormRequest formRequest);
        Task DeleteAsync(string id, string owner);
    }
}
