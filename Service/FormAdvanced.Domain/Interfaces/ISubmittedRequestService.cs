using FormAdvanced.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Interfaces
{
    public interface ISubmittedRequestService
    {
        Task<SubmittedRequest?> GetByIdAsync(string id, string owner);
        Task<List<SubmittedRequest>> GetByOwnerAsync(string owner);
        Task<List<SubmittedRequest>> GetByApproverAsync(string approver);
        Task<SubmittedRequest> UpsertAsync(SubmittedRequest submittedRequest);
        Task DeleteAsync(string id, string owner);
    }
}
