﻿using System.ComponentModel.DataAnnotations;

namespace ToDo.Web.Models.Requests;

public class LoginRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string Message { get; set; }
}
