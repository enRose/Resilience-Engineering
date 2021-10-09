using System;
using System.Threading.Tasks;
using api.Services;
using api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FaultTolerantController : ControllerBase
    {
        private readonly IAppService appService;

        public FaultTolerantController(IAppService x)
        {
            appService = x;
        }

        [Route("/app-in-flight")]
        [Route("/app-in-flight/{botId?}")]
        [HttpGet]
        public async Task<PersonalLoanVm> AppInFlight(int? botId) =>
            await appService.GetApp();


        // Called by client when user consents to be notified
        // by an in-app notification when leaf node is back online.
        [HttpPut]
        public async Task<bool> NotificationConsent() =>
            await appService.NotificationConsent();

        [Route("/app-landing")]
        [HttpGet]
        public async Task<PersonalLoanVm> GetAppLanding() => await appService.GetApp();

        // called by user when they agreed for retry
        [HttpGet]
        public async Task<bool> AgreeToRetry() => await appService.AgreeToRetry();

        // called by kafka consumer
        [HttpPost]
        public async Task<bool> ConsumerRetry(
            [FromBody] PersonalLoanVm v) => await appService.ConsumerRetry(v);
    }
}
