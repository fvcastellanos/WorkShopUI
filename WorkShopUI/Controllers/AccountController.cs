using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkShopUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _audience;

        public AccountController(IConfiguration configuration)
        {
            _audience = configuration["Auth0:Audience"];
        }

        [HttpGet]
        [Route("/login")]
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            // Indicate here where Auth0 should redirect the user after a login.
            // Note that the resulting absolute Uri must be added to the
            // **Allowed Callback URLs** settings for the app.
            .WithAudience(_audience)
            .WithRedirectUri(returnUrl)
            .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);

            var token = await HttpContext.GetTokenAsync("access_token");
        }

        // [Authorize]
        // public IActionResult Profile()
        // {
        //     return new View( new {
                
        //         Name = User.Identity.Name,
        //         EmailAddress = User.Claims
        //             .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
        //         ProfileImage = User.Claims
        //             .FirstOrDefault(c => c.Type == "picture")?.Value
        //     });
        // }

        [Authorize]
        [Route("/logout")]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            // Indicate here where Auth0 should redirect the user after a logout.
            // Note that the resulting absolute Uri must be added to the
            // **Allowed Logout URLs** settings for the app.
            .WithRedirectUri(Url.Action("Index", "Home"))
            .Build();

            // Logout from Auth0
            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            // Logout from the application
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }        
    }
}