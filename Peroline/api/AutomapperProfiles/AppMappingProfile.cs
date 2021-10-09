using System;
using AutoMapper;
using api.Entities;
using api.ViewModels;

namespace api.AutomapperProfiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<PersonalLoanVm, PersonalLoan>();
        }
    }
}
