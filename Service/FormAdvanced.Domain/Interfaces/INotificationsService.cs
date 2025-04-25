using FormAdvanced.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Interfaces
{
    public interface INotificationsService
    {
        Task<Notification?> GetByIdAsync(string id, string owner);
        Task<List<Notification>> GetByOwnerAsync(string owner);
        Task<Notification> UpsertAsync(Notification notification);
        Task DeleteAsync(string id, string owner);
    }
}
