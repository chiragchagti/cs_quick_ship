/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using Domain.Entities;
using Application.Interfaces;
using Application.OauthRequest;
using Application.Features.User.Queries.LoginUserByOpenId;

namespace cs_quick_ship_authentication_server.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizeResultService _authorizeResultService;
        private readonly ICodeStoreService _codeStoreService;
        private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;

        public HomeController(IHttpContextAccessor httpContextAccessor, IAuthorizeResultService authorizeResultService,
            ICodeStoreService codeStoreService,
            IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizeResultService = authorizeResultService;
            _codeStoreService = codeStoreService;
            _userClaimsPrincipalFactory= userClaimsPrincipalFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Authorize the client app and if user is already authenticated generate token otherwise redirect to login view.
        /// </summary>
        /// <param name="authorizationRequest"></param>
        /// <returns></returns>
        public IActionResult Authorize(AuthorizationRequest authorizationRequest)
        {
            var result = _authorizeResultService.AuthorizeRequest(_httpContextAccessor, authorizationRequest);

            if (result.HasError)
                return RedirectToAction("Error", new { error = result.Error });

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var updateCodeResult = _codeStoreService.UpdatedClientDataByCode(result.Code, 
                    _httpContextAccessor.HttpContext.User, result.RequestedScopes);
                if (updateCodeResult != null)
                {
                    result.RedirectUri = result.RedirectUri + "&code=" + result.Code;
                    return Redirect(result.RedirectUri);
                }
                else
                {
                    return RedirectToAction("Error", new { error = "invalid_request" });
                }

            }

            var loginModel = new LoginByOpenIdQuery("", "", result.RedirectUri, result.Code, result.RequestedScopes);

            return View("Login", loginModel);
        }

        [HttpGet]
        public IActionResult Login()
			{
			return View();
        }
        /// <summary>
        /// Login user and create claims.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginByOpenIdQuery loginRequest)
        {

            var userLoginResult = await Mediator.Send(loginRequest);

            if (userLoginResult.Succeeded)
            {
                var claimsPrincipals = await _userClaimsPrincipalFactory.CreateAsync(userLoginResult.AppUser);
                var result = _codeStoreService.UpdatedClientDataByCode(loginRequest.Code,
                    claimsPrincipals, loginRequest.RequestedScopes);
                if (result != null)
                {
                    var redirectUri = loginRequest.RedirectUri + "&code=" + loginRequest.Code;
                    return Redirect(redirectUri);
                }
            }
            return View("Error", new { error = "invalid_request" });
            //return RedirectToAction("Error", new { error = "invalid_request" });
        }

        /// <summary>
        /// Creating access token for the user
        /// </summary>
        /// <param name="tokenRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Token(TokenRequest tokenRequest)
        {
            var result = _authorizeResultService.GenerateToken(tokenRequest);

            if (result.HasError)
                return Json(new
                {
                    error = result.Error,
                    error_description = result.ErrorDescription
                });

            return Json(result);
        }

        public IActionResult Error(string error)
        {
            return View(error);
        }
    }
}
