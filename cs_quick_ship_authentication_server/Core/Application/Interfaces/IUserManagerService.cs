﻿/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */


using Domain.Entities;
using Application.OauthResponse;
using Application.OauthRequest;

namespace Application.Interfaces
{
    public interface IUserManagerService
    {
        Task<AppUser> GetUserAsync(string userId);
        Task<LoginResponse> LoginUserAsync(LoginRequest request);
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
        Task<OpenIdConnectLoginResponse> LoginUserByOpenIdAsync(OpenIdConnectLoginRequest request);
    }
}
