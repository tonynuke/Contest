﻿using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Services.Contest;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace Services
{
    /// <summary>
    /// Vk callback handler.
    /// </summary>
    public class VkCallbackHandler : IVkCallbackHandler
    {
        private const string Ok = "ok";

        private readonly ConfigurationsService _configurationsService;
        private readonly IVkApi _vkApi;
        private readonly IContestService _contestService;
        private readonly ILogger<VkCallbackHandler> _logger;

        public VkCallbackHandler(
            ConfigurationsService configurationsService,
            IVkApi vkApi,
            IContestService contestService,
            ILogger<VkCallbackHandler> logger)
        {
            _configurationsService =
                configurationsService ?? throw new ArgumentNullException(nameof(configurationsService));
            _vkApi = vkApi ?? throw new ArgumentNullException(nameof(vkApi));
            _contestService = contestService ?? throw new ArgumentNullException(nameof(contestService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<Result<string>> Handle(VkEvent vkEvent)
        {
            if (vkEvent.Type == VkEventType.Confirmation)
            {
                return await _configurationsService.ConfirmServerAddress(vkEvent.GroupId);
            }

            switch (vkEvent.Type)
            {
                case VkEventType.Confirmation:
                    await _configurationsService.ConfirmServerAddress(vkEvent.GroupId);
                    break;
                case VkEventType.NewMessage:
                    await Reply(vkEvent.Object);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Result.Success(Ok);
        }

        private async Task Reply(JToken json)
        {
            var vkResponse = new VkResponse(json);
            var message = Message.FromJson(vkResponse);

            if (!message.ChatId.HasValue || !message.UserId.HasValue)
            {
                _logger.LogCritical("There are no ChatId or UserId");
                return;
            }

            var contestContext = new ContestContext(message.ChatId.Value, message.UserId.Value, message.Text);
            var playResult = await _contestService.PlayContest(contestContext);

            var messagesSendParams = new MessagesSendParams
            {
                RandomId = default(DateTime).Millisecond,
                PeerId = message.PeerId.Value,
                Message = playResult
            };
            await _vkApi.Messages.SendAsync(messagesSendParams);
        }
    }
}