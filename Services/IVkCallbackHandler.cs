using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Services
{
    /// <summary>
    /// Vk callback handler interface.
    /// </summary>
    public interface IVkCallbackHandler
    {
        /// <summary>
        /// Handle vk event.
        /// </summary>
        /// <param name="vkEvent">Vk event.</param>
        /// <returns>Result.</returns>
        Task<Result<string>> Handle(VkEvent vkEvent);
    }
}