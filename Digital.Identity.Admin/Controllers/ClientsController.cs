using Digital.Identity.Admin.Services.Clients;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Controllers
{
    public class ClientsController: AdminControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        [ProducesResponseType( StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var clients = await _clientService.GetAllAsync();
            return StatusCode(StatusCodes.Status200OK, clients);
        }

        [HttpGet]
        [Route("{clientid}")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] string clientid, [FromQuery] bool withDependencies = false)
        {
            try
            {
                var client = await _clientService.FindByClientId(clientid, withDependencies);
                return StatusCode(StatusCodes.Status200OK, client);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] Client client)
        {
            var added = await _clientService.AddAsync(client);
            return StatusCode(StatusCodes.Status200OK, added);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] Client client)
        {
            var updated = await _clientService.UpdateAsync(client);
            return StatusCode(StatusCodes.Status200OK, updated);
        }

        [Route("{clientid}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string clientid)
        {
            var isDeleted = await _clientService.DeleteAsync(clientid);
            return isDeleted ? StatusCode(StatusCodes.Status200OK) : StatusCode(StatusCodes.Status409Conflict);
        }

        [Route("{clientid}/allowedcorsorigins")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AllowedCorsOrigins(string clientid, [FromBody] List<ClientCorsOrigin> origins)
        {
            try
            {
                var client = await _clientService.SetAllowedCorsOriginsAsync(clientid, origins);
                return StatusCode(StatusCodes.Status200OK, client);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [Route("{clientid}/allowedscopes")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AllowedScopes(string clientid, [FromBody] List<ClientScope> scopes)
        {
            try
            {
                var client = await _clientService.SetAllowedScopesAsync(clientid, scopes);
                return StatusCode(StatusCodes.Status200OK, client);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [Route("{clientid}/allowedgranttypes")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AllowedGrantTypes(string clientid, [FromBody] List<ClientGrantType> grantypes)
        {
            try
            {
                var client = await _clientService.SetAllowedGrantTypesAsync(clientid, grantypes);
                return StatusCode(StatusCodes.Status200OK, client);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [Route("{clientid}/redirecturis")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RedirectUris(string clientid, [FromBody] List<ClientRedirectUri> redirecturis)
        {
            try
            {
                var client = await _clientService.SetRedirectUrisAsync(clientid, redirecturis);
                return StatusCode(StatusCodes.Status200OK, client);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [Route("{clientid}/postlogoutredirecturis")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostLogoutRedirectUris(string clientid, [FromBody] List<ClientPostLogoutRedirectUri> postlogoutredirecturis)
        {
            try
            {
                var client = await _clientService.SetPostLogoutRedirectUrisAsync(clientid, postlogoutredirecturis);
                return StatusCode(StatusCodes.Status200OK, client);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
