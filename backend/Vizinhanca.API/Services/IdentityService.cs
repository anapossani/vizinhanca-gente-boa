using System.Security.Claims;

namespace Vizinhanca.API.Services
{
    public class IdentityService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public IdentityService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int GetUserId()
        {
            var userIdClaim = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("Não foi possível identificar o usuário.");
            }

            return int.Parse(userIdClaim);
        }
    }
}