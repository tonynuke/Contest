using System;

namespace Domain
{
    /// <summary>
    /// Vk CallbackApi configuration.
    /// </summary>
    /// <remarks>
    /// CallbackApi should be configured to allow bot sending messages.
    /// </remarks>
    public class CallbackApiConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackApiConfiguration"/> class.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <param name="confirmationKey">Confirmation key.</param>
        /// <param name="groupId">Group id.</param>
        public CallbackApiConfiguration(string accessToken, string confirmationKey, long groupId)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            ConfirmationKey = confirmationKey ?? throw new ArgumentNullException(nameof(confirmationKey));
            GroupId = groupId;
        }

        /// <summary>
        /// Gets Id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets access token.
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Gets confirmation key.
        /// </summary>
        public string ConfirmationKey { get; }

        /// <summary>
        /// Gets a value indicating whether configuration is confirmed.
        /// </summary>
        public bool IsConfirmed { get; private set; }

        /// <summary>
        /// Gets group id.
        /// </summary>
        public long GroupId { get; }

        /// <summary>
        /// Confirms configuration.
        /// </summary>
        public void Confirm()
        {
            IsConfirmed = true;
        }
    }
}