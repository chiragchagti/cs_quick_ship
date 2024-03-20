using Application.Features.User.Commands;
using Application.OauthRequest;
using Application.OauthResponse;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.User.Queries.LoginUser
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<LoginQueryHandler> _logger;
        public LoginQueryHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<LoginQueryHandler> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var validationResult = ValidateLoginRequest(request);

            if (!validationResult)
            {
                _logger.LogInformation("validation for login process is failed {request}", request);
                return new LoginResponse { Error = "login process is failed" };
            }

            AppUser user = null;

            user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null && request.UserName.Contains("@"))
                user = await _userManager.FindByEmailAsync(request.UserName);

            if (user == null)
            {
                _logger.LogInformation("creditioanl {userName}", request.UserName);
                return new LoginResponse { Error = "No user has this creditioanl" };
            }

            await _signInManager.SignOutAsync();


            SignInResult loginResult = await _signInManager
                .PasswordSignInAsync(user, request.Password, false, false);

            if (loginResult.Succeeded)
            {
                return new LoginResponse { Succeeded = true };
            }

            return new LoginResponse { Succeeded = false, Error = "Login is not Succeeded" };
        }
        private static bool ValidateLoginRequest(LoginQuery request)
        {
            if (request.UserName == null || request.Password == null)
                return false;

            if (request.Password.Length < 8)
                return false;

            return true;
        }
    }
}
