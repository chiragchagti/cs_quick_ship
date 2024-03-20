/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.AspNetCore.Mvc;
using Application.OauthRequest;
using Application.Interfaces;
using Application.Features.User.Commands;
using Application.Features.User.Queries.LoginUser;

namespace cs_quick_ship_authentication_server.Controllers
{
    public class AccountsController : BaseController
    {


        [HttpGet]
        public IActionResult Login(bool? popup = false)
        {
			if (popup.GetValueOrDefault())
			{
				// Set view data to indicate this is a popup window
				ViewData["Popup"] = true;
			}
			return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginQuery request)
        {
            //var query = new LoginQuery(request.UserName, request.Password);
            var result = await Mediator.Send(request);
            if(result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(request);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateUserCommand request)
        {
            //var command = new CreateUserCommand(request.UserName, request.Password, request.Email, request.PhoneNumber);
            var result = await Mediator.Send(request);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(request);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
