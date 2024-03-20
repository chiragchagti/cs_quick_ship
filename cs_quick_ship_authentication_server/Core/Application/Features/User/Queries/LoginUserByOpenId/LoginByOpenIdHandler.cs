using Application.OauthRequest;
using Application.OauthResponse;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Queries.LoginUserByOpenId
{
    public class LoginByOpenIdHandler : IRequestHandler<LoginByOpenIdQuery, OpenIdConnectLoginResponse>
    {
        private readonly ILogger<LoginByOpenIdHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public LoginByOpenIdHandler(ILogger<LoginByOpenIdHandler> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<OpenIdConnectLoginResponse> Handle(LoginByOpenIdQuery request, CancellationToken cancellationToken)
        {
            bool validationResult = ValidateOpenIdLoginRequest(request);
            if (!validationResult)
            {
                _logger.LogInformation("login process is failed for request: {request}", request);
                return new OpenIdConnectLoginResponse { Error = "The login process is failed" };
            }
            AppUser user = null;

            user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null && request.UserName.Contains("@"))
                user = await _userManager.FindByEmailAsync(request.UserName);

            if (user == null)
            {
                _logger.LogInformation("creditioanl {userName}", request.UserName);
                return new OpenIdConnectLoginResponse { Error = "No user has this creditioanl" };
            }
            await _signInManager.SignOutAsync();


            Microsoft.AspNetCore.Identity.SignInResult loginResult = await _signInManager
                .PasswordSignInAsync(user, request.Password, false, false);

            if (loginResult.Succeeded)
            {
                return new OpenIdConnectLoginResponse { Succeeded = true, AppUser = user };
            }

            return new OpenIdConnectLoginResponse { Succeeded = false, Error = "Login is not Succeeded" };
        }
        private static bool ValidateOpenIdLoginRequest(LoginByOpenIdQuery request)
        {
            if (request.Code == null || request.UserName == null || request.Password == null)
                return false;
            return true;
        }
    }
}
