using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.ViewModels;

namespace api.Controllers
{
    /* 
       Existing routes that not protected by retry.
    */
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonalLoanController : ControllerBase
    {
        private readonly IAppService appService;

        public PersonalLoanController(IAppService x)
        {
            appService = x;
        }

        // default route
        [Route("/customers/{id}/app")]
        [HttpGet]
        public async Task<PersonalLoanVm> GetApp(int id) =>
            await appService.GetApp(id);

        [HttpPost]
        public async Task<bool> SubmitApp() =>
            await appService.SubmitApp();
    }
}