using Newtonsoft.Json;

namespace Services.Vk
{
    /// <summary>
    /// Vk event type.
    /// </summary>
    public enum VkEventType
    {
        /// <summary>
        /// Server address confirmation.
        /// </summary>
        [JsonProperty("confirmation")]
        Confirmation,

        /// <summary>
        /// New message.
        /// </summary>
        [JsonProperty("message_new")]
        NewMessage
    }
}