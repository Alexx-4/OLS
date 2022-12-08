using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenLatino.Admin.Application.Utils;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
using OpenLatino.Core.Domain.Services;

namespace OpenLatino.Admin.Server.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService clientService;
        private readonly ITokenService tokenService;
        public ClientsController(IClientService clientService, ITokenService tokenService)
        {
            this.clientService = clientService;
            this.tokenService = tokenService;
        }

        // GET: api/Clients
        [HttpGet]
        [Route("")]
        public IEnumerable<Client> Get()
        {
            return clientService.GetAll();
        }        

        [AllowAnonymous]
        [Route("Refresh")]
        [HttpPost]
        public ObjectResult RefreshToken()
        {
            //test headers
            if (!Request.Headers.ContainsKey("expired_token") || !Request.Headers.ContainsKey("update_key"))
                return BadRequest("Headers most specify expired_token and update_key");

            var expired_token = Request.Headers["expired_token"];
            var update_key = Request.Headers["update_key"];
            var handler = new JwtSecurityTokenHandler();

            //test if token is valid
            if (!handler.CanReadToken(expired_token))
                return BadRequest("Invalid token");
            var token = handler.ReadJwtToken(expired_token);

            //Test if token is expired
            var expDate = DateTime.FromFileTime(long.Parse(token.Claims.First(c => c.Type == "expiresFt").Value));
            if (DateTime.Now < expDate)
                return BadRequest("The token specifield as expired is not expired yet");

            string clientId = token.Claims.First(c => c.Type == "nameid").Value;
            var client = clientService.FindClient(clientId);

            //test client exist
            if (client == null)
                return BadRequest("The user specifield by the token doesn't exist or token is corrupted");

            //test if refresh token is valid
            if (client.UpdateKey != update_key)
                return BadRequest("Invalid refresh token");

            //test that the client is active
            if (!client.Active)
                return BadRequest("Specifield client is inactive");

            //test that the expired token is not a previous version
            if (client.AccessKey != expired_token)
                return BadRequest("Specifield expired_token is a previous version, a newer one exist");


            var newToken = tokenService.GetTokenFor(new ClientCredentialModel()
            {
                ClientId = client.Id, AllowedOrigen = client.AllowedOrigin,
                Name = client.Name, Active = true, ExpirationDate = DateTime.Now.Add(Helper.TIcketLifeTime).ToFileTimeUtc()
            });
            var newRefresh = clientService.getRandomUpdateKey();

            //update client information on database
            clientService.UpdateAccessKey(client.Id, newToken);
            clientService.Update_UpdateKey(client.Id, newRefresh);

            return Ok(new { access_token = newToken, update_key = newRefresh});
        }
    }
}
