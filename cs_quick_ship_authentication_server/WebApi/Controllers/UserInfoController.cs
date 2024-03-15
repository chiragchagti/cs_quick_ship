using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using System.Threading.Tasks;
using cs_quick_ship_authentication_server.Services;

namespace cs_quick_ship_authentication_server.Controllers
{
    [Route("api/[controller]")]
    //[EnableCors("UserInfoPolicy")]
    [ApiController]
    [AllowAnonymous]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;
        public UserInfoController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// Fetching user info using OIDC
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]

        public async Task<IActionResult> GetUserInfo()
        {
            var userInfo = await _userInfoService.GetUserInfoAsync();
            return Ok(userInfo);
        }
    }
}
