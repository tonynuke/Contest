using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services
{
    /// <summary>
    /// Событие Vk.
    /// </summary>
    public class VkEvent
    {
        /// <summary>
        /// Тип события.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VkEventType Type { get; set; }

        /// <summary>
        /// Объект, инициировавший событие.
        /// </summary>
        /// <remarks>Структура объекта зависит от типа уведомления.</remarks>
        [JsonPropertyName("object")]
        public JsonDocument Object { get; set; }

        /// <summary>
        /// ID сообщества, в котором произошло событие.
        /// </summary>
        [JsonPropertyName("group_id")]
        public long GroupId { get; set; }
    }
}