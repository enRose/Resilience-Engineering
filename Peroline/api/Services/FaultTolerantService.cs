﻿using System;
using System.Net;
using System.Threading.Tasks;
using api.ViewModels;
using peroline.FaultTolerance;

namespace api.Services
{
    public interface IFaultTolerantService
    {
        Task<PersonalLoanVm> GetApp(string customerId, string botId = null);
        Task<bool> SubmitApp();
        Task<bool> AgreeToRetry();
        Task<bool> ConsumerRetry(PersonalLoanVm app);
        Task<bool> NotificationConsent();
    }

    public class FaultTolerantService : IFaultTolerantService
    {
        private readonly IAppService appService;

        public FaultTolerantService(
            IAppService s)
        {
            appService = s;
        }

        public async Task<PersonalLoanVm> GetApp(string customerId, string botId = null)
        {
            var tunnel = new Tunnel();

            return await tunnel.RetryOn(
                HttpStatusCode.InternalServerError,
                HttpStatusCode.ServiceUnavailable
                )
                .ExecuteAsyn(async () => await appService.GetApp(customerId));

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
