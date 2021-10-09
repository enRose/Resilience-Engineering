using System;
using System.Net;
using System.Threading.Tasks;
using api.ViewModels;
using peroline.FaultTolerance;

namespace api.Services
{
    public interface IFaultTolerantService
    {
        Task<PersonalLoanVm> GetApp(int customerId, int? botId = null);
        Task<bool> SubmitApp();
        Task<bool> AgreeToRetry();
        Task<bool> ConsumerRetry(PersonalLoanVm app);
        Task<bool> NotificationConsent();
    }

    public class FaultTolerantService : IFaultTolerantService
    {
        public FaultTolerantService()
        {
        }

        [Memoriser(500, 503)]
        public Task<PersonalLoanVm> GetApp(int customerId, int? botId = null)
        {
            throw new NotImplementedException();
        }








        public Task<bool> AgreeToRetry()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConsumerRetry(PersonalLoanVm app)
        {
            throw new NotImplementedException();
        }

        

        public Task<bool> NotificationConsent()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubmitApp()
        {
            throw new NotImplementedException();
        }
    }
}
