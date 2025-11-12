using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizinhanca.API.DTOs;
using Vizinhanca.API.Services;

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService; 

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            var dashboardData = await _dashboardService.GetDashboardDataAsync(userId);

            if (dashboardData == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(dashboardData);
        }
    }
}