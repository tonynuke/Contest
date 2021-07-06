using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Contest;
using VkNet.Abstractions;
using VkNet.Enums.SafetyEnums;

namespace Services.Contest
{
    public interface IValidator
    {
        Task<Result> Validate(ContestBase contest, ContestContext context);
    }

    public class Validator : IValidator
    {
        /// <summary>
        /// Vk API client.
        /// </summary>
        private readonly IVkApi _vkApi;

        public Validator(IVkApi vkApi)
        {
            _vkApi = vkApi ?? throw new ArgumentNullException(nameof(vkApi));
        }

        public async Task<Result> Validate(ContestBase contest, ContestContext context)
        {
            string expectedMessagePattern = contest.Configuration.MessagePattern;
            if (!string.IsNullOrWhiteSpace(expectedMessagePattern))
            {
                // TODO: validate message.
                if (!context.Message.ToUpperInvariant().Contains(expectedMessagePattern))
                {
                    return Result.Failure($"Сообщение не содержит {expectedMessagePattern}.");
                }
            }

            bool isUserGroupMember = await IsUserGroupMember(context.VkUserId, contest.VkGroupId);
            if (!isUserGroupMember)
            {
                return Result.Failure("Вступите в группу.");
            }

            bool isUserLikedPost = await IsUserLikedPost(context.VkUserId, context.VkPostId);
            if (!isUserLikedPost)
            {
                return Result.Failure("Поставьте лайк.");
            }

            return Result.Success();
        }

        private async Task<bool> IsUserGroupMember(long userId, string groupId)
        {
            var groupMembers = await _vkApi.Groups.IsMemberAsync(groupId, userId, null, null);
            return groupMembers.Any(member => member.UserId == (ulong)userId);
        }

        private Task<bool> IsUserLikedPost(long userId, long postId)
        {
            return _vkApi.Likes.IsLikedAsync(LikeObjectType.Post, postId, userId);
        }
    }
}