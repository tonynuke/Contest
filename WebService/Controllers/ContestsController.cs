using System;
using Microsoft.AspNetCore.Mvc;
using Services.Contest;

namespace WebService.Controllers
{
    /// <summary>
    /// Contests controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContestsController : ControllerBase
    {
        private readonly IContestService _contestService;

        public ContestsController(IContestService contestService)
        {
            _contestService = contestService ?? throw new ArgumentNullException(nameof(contestService));
        }
    }
}