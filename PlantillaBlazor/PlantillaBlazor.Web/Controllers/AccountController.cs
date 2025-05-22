using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantillaBlazor.Services.Interfaces.Perfilamiento;

namespace PlantillaBlazor.Web.Controllers;

[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly IConfiguration _config;

    private bool _oauthEnabled = false;
    public AccountController
    (
        ISessionService sessionService,
        IConfiguration config
    )
    {
        _sessionService = sessionService;
        _config = config;
    }


    [IgnoreAntiforgeryToken]
    [HttpGet("signin-oidc")]
    public ActionResult LoginResponse()
    {
        _oauthEnabled = _config.GetSection("OpenIDConnectSettings").GetValue<bool>("Enabled");

        if (!_oauthEnabled) return Redirect("/");

        var user = HttpContext.User;

        return Redirect("/signin-oidc");
    }

    [HttpGet("Login")]
    public ActionResult Login()
    {
        _oauthEnabled = _config.GetSection("OpenIDConnectSettings").GetValue<bool>("Enabled");

        if (!_oauthEnabled) return Redirect("/");

        string returnUrl = "/api/Account/signin-oidc";

        var properties = GetAuthProperties(returnUrl);

        return Challenge(properties);
    }

    [Authorize]
    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        var sessionId = User.Claims.FirstOrDefault(c => c.Type == "IdSession")!.Value;

        await _sessionService.InhabilitarSession(long.Parse(sessionId));

        return SignOut(
            new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Original src:
    /// https://github.com/dotnet/blazor-samples/blob/main/8.0/BlazorWebOidc/BlazorWebOidc/LoginLogoutEndpointRouteBuilderExtensions.cs
    /// </summary>
    private static AuthenticationProperties GetAuthProperties(string? returnUrl)
    {
        const string pathBase = "/";

        // Prevent open redirects.
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = pathBase;
        }
        else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
        {
            returnUrl = new Uri(returnUrl, UriKind.Absolute).PathAndQuery;
        }
        else if (returnUrl[0] != '/')
        {
            returnUrl = $"{pathBase}{returnUrl}";
        }

        return new AuthenticationProperties { RedirectUri = returnUrl };
    }
}
