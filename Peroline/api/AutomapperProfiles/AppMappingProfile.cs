using System;
using AutoMapper;
using retry.Entities;
using retry.ViewModels;

namespace retry.AutomapperProfiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<PersonalLoanViewModel, PersonalLoan>();
        }
    }
}
