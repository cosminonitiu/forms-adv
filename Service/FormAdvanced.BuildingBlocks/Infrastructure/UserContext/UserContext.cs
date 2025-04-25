using Microsoft.AspNetCore.Http;
using System.Linq;


namespace FormAdvanced.BuildingBlocks.Infrastructure.UserContext
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string UserId
        {
            get
            {
                return _contextAccessor?.HttpContext.User.Claims.First(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            }
        }

        public string Name
        {
            get
            {
                return _contextAccessor?.HttpContext.User.Identity.Name;
            }
        }
    }
}
