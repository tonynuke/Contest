using System;

namespace Domain
{
    /// <summary>
    /// Vk CallbackApi configuration.
    /// </summary>
    public class CallbackApiConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackApiConfiguration"/> class.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <param name="confirmationKey">Confirmation key.</param>
        public CallbackApiConfiguration(string accessToken, string confirmationKey)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            ConfirmationKey = confirmationKey ?? throw new ArgumentNullException(nameof(confirmationKey));
        }

        public Guid Id { get; }

        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Confirmation key.
        /// </summary>
        public string ConfirmationKey { get; }

        /// <summary>
        /// Configuration is confirmed.
        /// </summary>
        public bool IsConfirmed { get; private set; }

        /// <summary>
        /// Confirm.
        /// </summary>
        public void Confirm()
        {
            IsConfirmed = true;
        }
    }
}