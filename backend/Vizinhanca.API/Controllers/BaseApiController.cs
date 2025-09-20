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
                return 1;
            }
        }
    }
}