using dxStudyJWT.Utili;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dxStudyJWT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Get(string userName, string pwd)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(pwd))
            return BadRequest(new { message = "username or password is incorrect" });

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
            new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
            // new Claim(ClaimTypes.Name, userName), //添加这一行则会把user name放入生成的token中的payload
            new Claim(ClaimTypes.NameIdentifier, userName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Cont.SecurityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: Cont.Domain,
            audience: Cont.Domain,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
            );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
