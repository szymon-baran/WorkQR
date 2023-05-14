﻿using AutoMapper;
using WorkQR.Application;
using WorkQR.Domain;

namespace WorkQR.WebAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Position, CompanyPositionDTO>();
            CreateMap<ApplicationUser, RegistrationCodeUserDTO>();
            CreateMap<Vacation, VacationRequestDTO>();
            CreateMap<VacationRequestVM, Vacation>();
        }
    }
}
