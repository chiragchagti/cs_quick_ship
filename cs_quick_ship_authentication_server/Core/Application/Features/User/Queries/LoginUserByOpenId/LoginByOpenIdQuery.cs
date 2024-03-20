using Application.OauthResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Queries.LoginUserByOpenId
{
    public record LoginByOpenIdQuery
        (
            string UserName,
            string Password,
            string RedirectUri,
            string Code,
            IList<string> RequestedScopes
        ) : IRequest<OpenIdConnectLoginResponse>;
}
