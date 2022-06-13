using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Basic.Controllers;

public class OAuthController : Controller
{
    [HttpPost]
    public IActionResult Authorize(
        string username,
        string redirect_uri,
        string state)
    {
        var code = "this_is_a_code";

        var query = new QueryBuilder();
        query.Add("state", state);
        query.Add("code", code);
        
        return Redirect($"{redirect_uri}{query.ToString()}");
    }
    
    [HttpGet]
    public IActionResult Authorize(
        string response_type,
        string client_id,
        string redirect_uri,
        string scope,
        string state)
    {
        var query = new QueryBuilder();
        query.Add("redirect_uri", redirect_uri);
        query.Add("state", state);
        return View(model: query.ToString());
    }

    public async Task<IActionResult> Token(
        string grant_type,
        string code,
        string redirect_uri,
        string client_id)
    {
        // some mechanism for validating the code
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
            DateTime.Now.AddHours(1),
            signingCredentials);

        var access_token = new JwtSecurityTokenHandler().WriteToken(token);

        var responseObject = new
        {
            access_token,
            token_type = "Bearer",
        };

        return Ok(responseObject);
    }
}