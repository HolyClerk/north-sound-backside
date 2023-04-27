﻿using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Domain.Responses;

public class AuthenticateResponse
{
    public AuthenticateResponse(User userEntity, string token)
    {
        UserEntity = userEntity;
        Token = token;
    }

    public User UserEntity { get; set; }

    public string Token { get; set; }
}
