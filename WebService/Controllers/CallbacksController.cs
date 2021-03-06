using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebService.Controllers
{
    /// <summary>
    /// Vk Callback API controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CallbacksController : ControllerBase
    {
        private readonly IVkCallbackHandler _callbackHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbacksController"/> class.
        /// </summary>
        /// <param name="callbackHandler">Callback handler.</param>
        public CallbacksController(IVkCallbackHandler callbackHandler)
        {
            _callbackHandler = callbackHandler ?? throw new ArgumentNullException(nameof(callbackHandler));
        }

        /// <summary>
        /// Processes vk event.
        /// </summary>
        /// <param name="vkEvent">Vk event.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Callback([FromBody] VkEvent vkEvent)
        {
            var result = await _callbackHandler.Handle(vkEvent);
            return result.IsSuccess
                ? Ok(result.Value)
                : Conflict();
        }
    }
}
