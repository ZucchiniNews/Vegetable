using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.UsersService;
using Zucchinimvc.Controllers.ApiInternal.Filters;

namespace Zucchinimvc.Controllers.ApiInternal
{
    [Route("api/internal/users")]
    [ApiController]
    [ApiKeyAuth]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("subscribed")]
        public async Task<IActionResult> GetNewsLetterSubscribedUsers()
        {
            var users = await _userService.GetNewsletterSubscribersAsync();
            return Ok(users);
        }
    }
}
