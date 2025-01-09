﻿using AuthenticationServer.Models.Requests;
using AuthenticationServer.Services;
using ToDo.Identity.Data;
using ToDo.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.Controllers;

public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly TokenGenerator _tokenGenerator;

    public AuthController(IUserRepository userRepository, PasswordHasher passwordHasher, TokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        UserProfile user = await _userRepository.GetByEmailAsync(loginRequest.Email);
        if (user == null)
        {
            return Unauthorized();
        }

        bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
        if (!isCorrectPassword)
        {
            return Unauthorized();
        }

        string accessToken = _tokenGenerator.GenerateToken(user);

        return Ok(accessToken);
    }
}
