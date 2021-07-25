using AutoMapper;
using com.allcard.institution.common;
using com.allcard.institution.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            DisableConstructorMapping();

            CreateMap<Institution, institutionVM>();
            CreateMap<institutionVM, Institution>();

             CreateMap<Chain, chainVM>();
            CreateMap<chainVM, Chain>();

             CreateMap<Group, groupVM>()
                .ForMember(x => x.Chain, opt => opt.MapFrom(src => src.Chain.Name));
            CreateMap<groupVM, Group>();

             CreateMap<Merchant, merchantVM>()
                .ForMember(x => x.Group, opt => opt.MapFrom(src => src.Group.Name));
            CreateMap<merchantVM, Merchant>();

             CreateMap<Branch, branchVM>()
                .ForMember(x => x.Merchant, opt => opt.MapFrom(src => src.Merchant.Name))
                 .ForMember(x => x.Institution, opt => opt.MapFrom(src => src.Merchant.Group.Chain.Name));
            CreateMap<branchVM, Branch>();

              CreateMap<Location, locationVM>()
                .ForMember(x => x.Branch, opt => opt.MapFrom(src => src.Branch.Name));
            CreateMap<locationVM, Location>();


            CreateMap<BranchSchedule, branchScheduleVM>()
              .ForMember(x => x.Branch, opt => opt.MapFrom(src => src.Branch.Name));
            CreateMap<branchScheduleVM, BranchSchedule>();

            CreateMap<BranchScheduleMember, branchScheduleMemberVM>();
            CreateMap<branchScheduleMemberVM, BranchScheduleMember>();

            CreateMap<Member, memberVM>();
            CreateMap<memberVM, Member>();

            CreateMap<UsersProfile, userProfileVM>()
                .ForMember(x => x.Branch, opt => opt.MapFrom(src => src.Branch.Name));
            CreateMap<userProfileVM, UsersProfile>();

            CreateMap<RefCityMunicipality, refCityMunicipalityVM>();
            CreateMap<refCityMunicipalityVM, RefCityMunicipality>();
        }
    }
}
