using System.Security.Claims;
using Microsoft.AspNetCore.Components;

namespace WorkShopUI.Pages
{
    public class UserInformationBase : ComponentBase
    {
        [Inject]
        protected IHttpContextAccessor HttpContextAccesor { get; set; }

        protected IDictionary<string, string> UserInformation;

        protected override void OnInitialized()
        {
            UserInformation = buildUserInformation();
            base.OnInitialized();
        }

        private IDictionary<string, string> buildUserInformation() 
        {
            var httpContext = HttpContextAccesor.HttpContext;
            var claims = httpContext.User.Claims;
            
            return new Dictionary<string, string>
            {
                { "name", httpContext.User.Identity.Name },
                { "picture", getClaimValue("picture", claims)},
                { "nickname", getClaimValue("nickname", claims)}
            };
        }

        private string getClaimValue(string claimName, IEnumerable<Claim> identities)
        {
            return identities.Filter(claim => claim.Type.Equals(claimName))
                .FirstOrDefault()?.Value;
        }
    }
}