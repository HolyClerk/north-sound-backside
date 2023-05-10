﻿using NorthSound.Backend.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.Responses;

public class RegisterRequest
{
    [Required] public string Username { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required] public string Password { get; set; }

    public UserDTO MapToUser()
    {
        return new UserDTO()
        {
            Name = Username,
            Email = Email,
            Password = Password,
        };
    }
}