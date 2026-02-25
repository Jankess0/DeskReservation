using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeskReservation.IntegrationTests;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Brak nagłówka."));
        }
        
        Claim[] claims;
        if (Scheme.Name == "TestAdmin")
        {
            claims = new[]
            {
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Admin")
            };
        }
        else if (Scheme.Name == "TestUser")
        {
            claims = new[] 
            { 
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.NameIdentifier, "1"), 
                new Claim(ClaimTypes.Role, "User") 
            };
        }
        else
        {
            return Task.FromResult(AuthenticateResult.Fail("Nieznany schemat."));
        }

        
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}