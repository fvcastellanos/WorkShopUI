using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WorkShopUI.Authentication;
using WorkShopUI.Authentication.Domain;
using WorkShopUI.Authentication.Provider;
using WorkShopUI.Domain.Views;

namespace WorkShopUI.Controllers
{
    // [Route("/login")]
    public class LoginController // : Controller
    {

    //     private readonly HttpContext _httpContext;

    //     private readonly ILogger _logger;

    //     private readonly AuthClient _authClient;

    //     private readonly TokenProvider _tokenProvider;

    //     public LoginController(ILogger<LoginController> logger, 
    //                            AuthClient authClient,
    //                            TokenProvider tokenProvider,
    //                            IHttpContextAccessor httpContextAccessor)
    //     {
    //         _logger = logger;
    //         _authClient = authClient;
    //         _tokenProvider = tokenProvider;
    //         _httpContext = httpContextAccessor.HttpContext;
    //     }

    //     // [HttpGet]
    //     public async Task<IActionResult> Index()
    //     {

    //         if (Request.Query.ContainsKey("logout"))
    //         {
    //             await _tokenProvider.DeleteTokenInformationAsync(_httpContext.User.Identity.Name);
    //             await _httpContext.SignOutAsync();
    //         }

    //         return View("Login");
    //     } 

    //     // [HttpPost]
    //     public async Task<IActionResult> Login(LoginModel login)
    //     {
    //         if (ModelState.IsValid)
    //         {
    //             if (await AuthenticateUserAsync(login))
    //             {
    //                 return Redirect("/");
    //             }
    //         }

    //         ModelState.AddModelError("LoginError", $"Error al autenticar al usuario: {login.User}");
    //         return View("Login");
    //     }       

    //     // -----------------------------------------------------------------------------------------------

    //     private async Task<bool> AuthenticateUserAsync(LoginModel loginModel)
    //     {
    //         try
    //         {
    //             var authResponse = await _authClient.PerformAuthentication(loginModel.User, loginModel.Password);
    //             var userInfo = await _authClient.GetUserInfoAsync(authResponse.AccessToken);

    //             // var response = new LoginResponse
    //             // {
    //             //     Jwt = "jwt",
    //             //     User = new User
    //             //     {
    //             //         Email = "jpenas@mail.net",
    //             //         Username = "jpenas",
    //             //         Id = 1
    //             //     }
    //             // };

    //             // var principal = BuildClaimsPrincipal(response);
    //             var principal = BuildClaimsPrincipal(userInfo);
    //             var authenticationProperties = new AuthenticationProperties()
    //             {
    //                 IsPersistent = true
    //             };

    //             await _httpContext.SignInAsync(principal, authenticationProperties);
    //             await _tokenProvider.StoreTokenInformationAsync(loginModel.User, authResponse);

    //             _logger.LogInformation("Success authentication for user: {0}", loginModel.User);

    //             return true;

    //         }
    //         catch (Exception exception)
    //         {
    //             _logger.LogError(exception, $"Unable to authenticate User: {loginModel.User}");
    //             return false;
    //         }
    //     }

    //     private ClaimsPrincipal BuildClaimsPrincipal(AuthUserInfo userInfo)
    //     {
    //         var claims = new Claim[] {
    //             new Claim(ClaimTypes.Name, userInfo.Name),
    //             new Claim(ClaimTypes.Email, userInfo.Email),
    //         };

    //         var identity = new ClaimsIdentity(claims, "Resource Owner");
    //         return new ClaimsPrincipal(identity);
    //    }
    }
}