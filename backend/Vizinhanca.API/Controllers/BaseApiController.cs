// Em Controllers/BaseApiController.cs
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; 

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {

        protected int UsuarioLogadoId
        {
            get
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    throw new InvalidOperationException("Não foi possível identificar o usuário logado a partir do token.");
                }

                return int.Parse(userIdClaim);                
                
            }
        }
    }
}