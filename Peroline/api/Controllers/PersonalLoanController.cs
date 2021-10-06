using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using retry.Services;
using retry.ViewModels;

namespace retry.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonalLoanController : ControllerBase
    {
        private readonly IAppService appService;

        public PersonalLoanController(IAppService y)
        {
            appService = y;
        }

        // default route
        [Route("/app")]
        [HttpGet]
        public async Task<PersonalLoanVm> GetApp() => await appService.GetApp();

        // this route throws transient errors
        [HttpPost]
        public async Task<bool> SubmitApp() => await appService.SubmitApp();

        // called by user when they agreed for retry
        [HttpGet]
        public async Task<bool> AgreeToRetry() => await appService.AgreeToRetry();

        // called by kafka consumer
        [HttpPost]
        public async Task<bool> ConsumerRetry(
            [FromBody] PersonalLoanVm v) => await appService.ConsumerRetry(v);
    }
}
