using AutoMapper;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Services.Clients
{
    public class ClientService: IClientService
    {
        private readonly ConfigurationDbContext _configDb;
        private readonly IMapper _mapper;

        public ClientService(ConfigurationDbContext configDb)
        {
            _configDb = configDb;
        }

        public async Task<IList<Client>> GetAllAsync()
        {
            var clientsEntities = await _configDb.Clients.ToListAsync();
            return clientsEntities;
        }

        public async Task<Client> FindByClientId(string clientId, bool withDependencies)
        {
            var clientQuery = _configDb.Clients.AsQueryable();
            if (withDependencies)
            {
                clientQuery =clientQuery.Include(c => c.AllowedCorsOrigins)
                    .Include(c => c.AllowedScopes)
                    .Include(c => c.ClientSecrets)
                    .Include(c => c.AllowedGrantTypes)
                    .Include(c => c.RedirectUris)
                    .Include(c => c.PostLogoutRedirectUris)
            }

            var client = await clientQuery.FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (client == null) throw new KeyNotFoundException($"A client with clientId: {clientId} does not exist.");

            return client;
        }

        public async Task<Client> AddAsync(Client client)
        {
            var added = await _configDb.AddAsync(client);
            await _configDb.SaveChangesAsync();

            return added.Entity;
        }

        public async Task<Client> UpdateAsync(Client client)
        {
            var updated = _configDb.Update(client);
            await _configDb.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<bool> DeleteAsync(string clientId)
        {
            var client = await _configDb.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null) throw new KeyNotFoundException($"A client with clientId: {clientId} does not exist.");

            _configDb.Remove(client);
            var deleted = await _configDb.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<Client> SetAllowedCorsOriginsAsync(string clientId, List<ClientCorsOrigin> origins)
        {
            var client = await _configDb.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null) throw new KeyNotFoundException($"A client with clientId: {clientId} does not exist.");

            client.AllowedCorsOrigins = origins;
            var updated = _configDb.Clients.Update(client);
            await _configDb.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Client> SetAllowedScopesAsync(string clientId, List<ClientScope> scopes)
        {
            var client = await _configDb.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null) throw new KeyNotFoundException($"A client with clientId: {clientId} does not exist.");

            client.AllowedScopes = scopes;
            var updated = _configDb.Clients.Update(client);
            await _configDb.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Client> SetClientSecretsAsync(string clientId, List<ClientSecret> secrets)
        {
            var client = await _configDb.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null) throw new KeyNotFoundException($"A client with clientId: {clientId} does not exist.");

            client.ClientSecrets = secrets;
            var updated = _configDb.Clients.Update(client);
            await _configDb.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Client> SetAllowedGrantTypesAsync(string clientId, List<ClientGrantType> grantTypes)
        {
            var client = await _configDb.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null) throw new KeyNotFoundException($"A client with clientId: {clientId} does not exist.");

            client.AllowedGrantTypes = grantTypes;
            var updated = _configDb.Clients.Update(client);
            await _configDb.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Client> SetRedirectUrisAsync(string clientId, List<ClientRedirectUri> redirectUris)
        {
            var client = await _configDb.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null) throw new KeyNotFoundException($"A client with clientId: {clientId} does not exist.");

            client.RedirectUris = redirectUris;
            var updated = _configDb.Clients.Update(client);
            await _configDb.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Client> SetPostLogoutRedirectUrisAsync(string clientId, List<ClientPostLogoutRedirectUri> postLogoutRedirectUris)
        {
            var client = await _configDb.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null) throw new KeyNotFoundException($"A client with clientId: {clientId} does not exist.");

            client.PostLogoutRedirectUris = postLogoutRedirectUris;
            var updated = _configDb.Clients.Update(client);
            await _configDb.SaveChangesAsync();

            return updated.Entity;
        }

    }
}
