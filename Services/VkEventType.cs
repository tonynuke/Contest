using System.Text.Json.Serialization;

namespace Services
{
    /// <summary>
    /// Тип события Vk.
    /// </summary>
    public enum VkEventType
    {
        /// <summary>
        /// Подтверждение адреса сервера.
        /// </summary>
        [JsonPropertyName("confirmation")]
        Confirmation,
    }
}