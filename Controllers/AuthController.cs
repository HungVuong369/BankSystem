using BankSystem.Dtos.Request;
using BankSystem.Dtos.Response;
using BankSystem.Service;
using BankSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        private void setTokenCookie(string refreshToken, DateTime? expireDateTime = null)
        {
            CookieOptions cookieOptions = null;

            // Config refresh token
            if (expireDateTime == null)
            {
                cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(2),
                };
            }
            else
            {
                TimeSpan timeDifference = expireDateTime.Value - DateTime.Now;
                int secondsDifference = (int)timeDifference.TotalSeconds;

                cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddSeconds(secondsDifference),
                };
            }

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [AllowAnonymous]
        [HttpPost("authenticateUser")]
        public IActionResult AuthenticateUser([FromBody] AuthenticateUserRequest authenticateUserRequest)
        {
            try
            {
                var result = _authService.AuthenticateUser(authenticateUserRequest);

                if (result.ErrorCode == 17)
                {
                    return BadRequest(HelperFunctions.Instance.GetErrorResponseByError(result.ErrorCode));
                }

                if (Request.Cookies["refreshToken"] == null)
                {
                    setTokenCookie((result.Data as UserTokenResponse).RefreshToken);
                }
                else
                {
                    var refreshToken = Request.Cookies["refreshToken"];
                    var responseRefreshToken = (result.Data as UserTokenResponse).RefreshToken;

                    if (refreshToken != responseRefreshToken)
                    {
                        setTokenCookie((result.Data as UserTokenResponse).RefreshToken);
                    }
                    else
                    {
                        setTokenCookie((result.Data as UserTokenResponse).RefreshToken, (result.Data as UserTokenResponse).ExpireDateTimeRefreshToken);
                    }
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(string inputRefreshToken)
        {
            var result = _authService.RefreshToken(inputRefreshToken);

            if (result.ErrorCode != 0)
                return BadRequest(result);

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRequest userRequest)
        {
            var result = _authService.Register(userRequest);
            return StatusCode(result.ErrorCode, result);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var result = _authService.Logout(Request.Cookies["refreshToken"].ToString());

            if (result.ErrorCode == 0)
            {
                Response.Cookies.Delete("refreshToken");
                Response.Cookies.Delete("accessToken");
            }
            return StatusCode(result.ErrorCode, result);
        }
    }
}
