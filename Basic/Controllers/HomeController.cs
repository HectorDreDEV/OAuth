using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Basic.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    [Authorize]
    public IActionResult Secret()
    {
        return View();
    }

    public IActionResult Authenticate()
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
            new Claim("granny", "cookie")
        };

        var secretBytes = Encoding.UTF8.GetBytes(Contants.Secret);
        var key = new SymmetricSecurityKey(secretBytes);
        var algorithm = SecurityAlgorithms.HmacSha256;

        var signingCredentials = new SigningCredentials(key, algorithm);
        
        var token = new JwtSecurityToken(Contants.Issuer, 
                                         Contants.Audience, 
                                         claims,
                                         notBefore: DateTime.Now,
                                         DateTime.Now.AddDays(1),
                                         signingCredentials);

        var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(tokenJson);
    }
}