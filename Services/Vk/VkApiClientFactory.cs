using System.Collections.Generic;
using System.Linq;
using Domain;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace Services.Vk
{
    public interface IVkApiClientFactory
    {
        /// <summary>
        /// Gets client for group.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>Vk API client.</returns>
        IVkApi GetClient(long groupId);
    }

    public class VkApiClientFactory : IVkApiClientFactory
    {
        private readonly Dictionary<long, IVkApi> _vkClientDictionary;

        public VkApiClientFactory(IEnumerable<VkCallbackApiConfiguration> configurations)
        {
            var apiGroupsClients = configurations.Select(configuration =>
            {
                var apiClient = new VkApi();
                var authParams = new ApiAuthParams
                {
                    AccessToken = configuration.AccessToken
                };
                apiClient.Authorize(authParams);
                return new
                {
                    Client = (IVkApi)apiClient,
                    GroupId = configuration.GroupId
                };
            });

            _vkClientDictionary = apiGroupsClients.ToDictionary(
                groupClient => groupClient.GroupId, groupClient => groupClient.Client);
        }

        public IVkApi GetClient(long groupId)
        {
            return _vkClientDictionary[groupId];
        }
    }
}