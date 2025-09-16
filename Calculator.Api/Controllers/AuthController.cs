using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Calculator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration cfg) : ControllerBase
{
    private readonly IConfiguration _cfg = cfg;

    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult IssueToken([FromQuery] string username = "demo")
    {
        var issuer = _cfg["Jwt:Issuer"];
        var audience = _cfg["Jwt:Audience"];
        var key = _cfg["Jwt:Key"];
        System.Console.WriteLine(key);
        if (key == null)
        {
            throw new NullReferenceException();
        }
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var now = DateTime.UtcNow;

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: new[] { new Claim(JwtRegisteredClaimNames.Sub, username) },
            notBefore: now,
            expires: now.AddMinutes(15),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = jwt, expiresAt = token.ValidTo });
    }
}
