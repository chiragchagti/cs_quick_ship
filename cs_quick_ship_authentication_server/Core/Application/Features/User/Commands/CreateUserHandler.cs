using Application.Interfaces;
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

namespace Application.Features.User.Commands
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CreateUserHandler> _logger;


        public CreateUserHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = ValidateCreateUserRequest(request);

            if (!validationResult)
            {
                _logger.LogInformation("The create user request is failed please check your input {request}", request);
                return new CreateUserResponse { Error = "The create user request is failed please check your input" };
            }


            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = request.PhoneNumber,
                TwoFactorEnabled = false,
            };
            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (createUserResult.Succeeded)
                return new CreateUserResponse { Succeeded = true };

            return new CreateUserResponse { Error = createUserResult.Errors.Select(x => x.Description).FirstOrDefault() };
        }

        private static bool ValidateCreateUserRequest(CreateUserCommand request)
        {
            if (request.UserName == null || request.Password == null || request.Email == null)
                return false;
            return true;
        }
    }
}
