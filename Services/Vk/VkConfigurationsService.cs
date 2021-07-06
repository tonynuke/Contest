using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services.Vk
{
    public class VkConfigurationsService
    {
        private readonly ApplicationDbContext _dbContext;

        public VkConfigurationsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Confirms server address.
        /// </summary>
        /// <param name="groupId">Group ID.</param>
        /// <returns>Confirmation key.</returns>
        public Task<Result<string>> ConfirmServerAddress(long groupId)
        {
            return Get(groupId)
                .ToResult($"Configuration with groupId {groupId} not found")
                .Tap(configuration => configuration.Confirm())
                .Tap(_ => _dbContext.SaveChangesAsync())
                .Map(configuration => configuration.ConfirmationKey);
        }

        private async Task<Maybe<VkCallbackApiConfiguration>> Get(long groupId)
        {
            var result = await _dbContext.CallbackApiConfigurations
                .SingleOrDefaultAsync(configuration => configuration.GroupId == groupId);

            return Maybe<VkCallbackApiConfiguration>.From(result);
        }
    }
}