using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services.Vk
{
    /// <summary>
    /// Vk event.
    /// </summary>
    public class VkEvent
    {
        /// <summary>
        /// Gets event type.
        /// </summary>
        [JsonProperty("type")]
        public VkEventType Type { get; init; }

        /// <summary>
        /// Gets object that had initiated event.
        /// </summary>
        /// <remarks>Object structure depends on event type.</remarks>
        [JsonProperty("object")]
        public JToken Object { get; init; }

        /// <summary>
        /// Gets group Id, where event had happened.
        /// </summary>
        [JsonProperty("group_id")]
        public long GroupId { get; init; }
    }
}