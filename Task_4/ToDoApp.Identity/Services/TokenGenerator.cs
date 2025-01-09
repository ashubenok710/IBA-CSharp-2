﻿using AuthenticationServer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDo.Identity.Models;

namespace AuthenticationServer.Services;

public class TokenGenerator
{
    private readonly AuthenticationConfiguration _authenticationConfiguration;

    public TokenGenerator(AuthenticationConfiguration authenticationConfiguration)
    {
        _authenticationConfiguration = authenticationConfiguration;
    }

    public string GenerateToken(UserProfile user)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfiguration.AccessTokenSecret));        

        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new List<Claim>()
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)            
        };

        JwtSecurityToken token = new JwtSecurityToken(
            _authenticationConfiguration.Issuer,
            _authenticationConfiguration.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_authenticationConfiguration.AccessTokenExpirationMinutes),
            credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
