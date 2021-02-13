using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallbackController : ControllerBase
    {
        private const string OkResult = "ok";

        private readonly ILogger<CallbackController> _logger;

        public CallbackController(ILogger<CallbackController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public IActionResult Callback([FromBody] VkEvent vkEvent)
        {
            //// Проверяем, что находится в поле "type" 
            //switch (vkEvent.Type)
            //{
            //    // Если это уведомление для подтверждения адреса
            //    case "confirmation":
            //        // Отправляем строку для подтверждения 
            //        return Ok(_configuration["Config:Confirmation"]);
            //}
            // Возвращаем "ok" серверу Callback API
            return Ok(OkResult);
        }
    }
}
