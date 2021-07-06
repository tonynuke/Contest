using System;

namespace Services.Contest
{
    /// <summary>
    /// Contest context.
    /// </summary>
    public class ContestContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContestContext"/> class.
        /// </summary>
        /// <param name="vkPostId">Vk post id.</param>
        /// <param name="vkUserId">Vk user id.</param>
        /// <param name="vkPeerId">Vk peer id.</param>
        /// <param name="message">Message.</param>
        public ContestContext(
            long vkPostId, long vkUserId, long vkPeerId, string message)
        {
            VkPostId = vkPostId;
            VkUserId = vkUserId;
            VkPeerId = vkPeerId;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <summary>
        /// Gets vk post id.
        /// </summary>
        public long VkPostId { get; }

        /// <summary>
        /// Gets vk user id.
        /// </summary>
        public long VkUserId { get; }

        /// <summary>
        /// Gets message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Get destination id.
        /// </summary>
        public long VkPeerId { get; }
    }
}