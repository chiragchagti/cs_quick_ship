using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Shared.DTOs;
using Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using MediatR;
using Application.Features.Shipment.Command;
using Application.Features.Shipment.Query;


namespace cs_quick_ship_resource_server.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class ShipmentController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration Configuration;
        private IMediator _mediator;
        public ShipmentController(IEmailService emailService, IConfiguration configuration, IMediator mediator)
        {
            _emailService = emailService;
            Configuration = configuration;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment(string addressFrom, string addressTo, int cariierId)
        {
            var command = new ShipmentCommand(addressFrom, addressTo, cariierId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("Shipments")]
        public async Task<IActionResult> GetShipments()
        {
            return Ok(await _mediator.Send(new ShipmentQuery()));
        }





























        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var test = Configuration["QuickStartApp:Settings:Message"];
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var email = HttpContext.User.FindFirst("email");
            string? res = await GetDataFromApi(accessToken ?? "", "https://localhost:7065", "/api/Articles/GetDummyData");
            string? res1 = await GetDataFromApi(accessToken ?? "", "http://localhost:7157", "/api/HttpTriggerCSharp");
        
            return Ok(res + " " + res1);
        }
        private async Task<string?> GetDataFromApi(string accessToken, string url, string endpoint)
        {
            using (var client = new HttpClient())
            {
                //string url = "https://localhost:7065";

                Uri baseUri = new Uri(url);
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                using var req = new HttpRequestMessage(HttpMethod.Get, endpoint);
                using var res = client.Send(req);

                res.EnsureSuccessStatusCode(); // If not success it will throw HttpRequestException
                string responseBody = await res.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
        // GET api/<controller>/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        //}

        //// POST api/<controller>
        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> Post(CreateProductCommand command)
        //{
        //    return Ok(await Mediator.Send(command));
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //[Authorize]
        //public async Task<IActionResult> Put(int id, UpdateProductCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await Mediator.Send(command));
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //[Authorize]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        //}
    }
}
