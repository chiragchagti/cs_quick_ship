using Application.OauthResponse;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Commands
{
    public record CreateUserCommand
        (
            string UserName,
            string Password,
            string Email,
            string PhoneNumber
        ) : IRequest<CreateUserResponse>;
}
