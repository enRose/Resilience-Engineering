using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace retry.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RetryController : ControllerBase
    {
        private readonly IYoyo yoyo;

        public RetryController(IYoyo y)
        {
            yoyo = y;
        }

        // this route throws transient errors
        [HttpGet]
        public async Task<bool> WriteGitHubBranch()
        {
            Logger.LogThreadInfo("Controller WriteGitHubBranch()");

            return await yoyo.WriteGitHubBranch();
        }

        // called by user when they agreed for retry
        [HttpGet]
        public async Task<bool> AgreeToRetry()
        {
            Logger.LogThreadInfo("Controller AgreeToRetry()");

            return await yoyo.AgreeToRetry();
        }

        // called by kafka consumer
        [HttpPost]
        public async Task<bool> ConsumerRetryWriteGitHubBranch(
            [FromBody] IEnumerable<GitHubBranch> gitHubBranches)
        {
            Logger.LogThreadInfo("Controller ConsumerRetryWriteGitHubBranch()");

            return await yoyo.ConsumerRetryWriteGitHubBranch(gitHubBranches);
        }

        // default route
        [Route("/Retry")]
        [HttpGet]
        public async Task<IEnumerable<GitHubBranch>> GetGitHubBranch()
        {
            Console.WriteLine("Controller GetGitHubBranch()");

            return await yoyo.GetGitHubBranch();
        }
    }
}
