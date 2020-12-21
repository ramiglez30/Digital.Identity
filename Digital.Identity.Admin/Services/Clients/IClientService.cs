using IdentityServer4.EntityFramework.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Services.Clients
{
    public interface IClientService
    {
        Task<IList<Client>> GetAllAsync();

        Task<Client> FindByClientId(string clientId, bool withDependencies);
        Task<Client> AddAsync(Client client);
        Task<Client> UpdateAsync(Client client);
        Task<bool> DeleteAsync(string clientId);
        Task<Client> SetAllowedCorsOriginsAsync(string clientId, List<ClientCorsOrigin> origins);
        Task<Client> SetAllowedScopesAsync(string clientId, List<ClientScope> scopes);
        Task<Client> SetClientSecretsAsync(string clientId, List<ClientSecret> secrets);
        Task<Client> SetAllowedGrantTypesAsync(string clientId, List<ClientGrantType> grantTypes);
        Task<Client> SetRedirectUrisAsync(string clientId, List<ClientRedirectUri> redirectUris);
        Task<Client> SetPostLogoutRedirectUrisAsync(string clientId, List<ClientPostLogoutRedirectUri> postLogoutRedirectUris);

    }
}
