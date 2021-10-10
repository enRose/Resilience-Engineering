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
        private readonly IFaultTolerantService faultTolerantService;

        public FaultTolerantController(IFaultTolerantService x) =>
            faultTolerantService = x;


        [Route("/customers/{customerId}/app-in-flight")]
        [Route("/customers/{customerId}//app-in-flight/{botId?}")]
        [HttpGet]
        public async Task<PersonalLoanVm> AppInFlight(
            string customerId,
            string botId) =>
            await faultTolerantService.GetApp(customerId, botId);


        // Called by client when user consents to be notified
        // by an in-app notification when leaf node is back online.
        [HttpPut]
        public async Task<bool> NotificationConsent() =>
            await faultTolerantService.NotificationConsent();






        [Route("/customers/{customerId}/app-landing")]
        [HttpGet]
        public async Task<PersonalLoanVm> GetAppLanding(string customerId) =>
            await faultTolerantService.GetApp(customerId);

        // called by user when they agreed for retry
        [HttpGet]
        public async Task<bool> AgreeToRetry() =>
            await faultTolerantService.AgreeToRetry();

        // called by kafka consumer
        [HttpPost]
        public async Task<bool> ConsumerRetry(
            [FromBody] PersonalLoanVm v) =>
            await faultTolerantService.ConsumerRetry(v);
    }
}
