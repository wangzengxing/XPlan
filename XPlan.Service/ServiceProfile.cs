using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using XPlan.Model.Plans;
using XPlan.Repository.Abstracts;

namespace XPlan.Service.Core
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Plan, PlanDto>()
                .ForMember(r => r.CreateTime, s => s.MapFrom(t => t.CreateTime.ToString("yyyy-MM-dd")));
            CreateMap<Page<Plan>, Page<PlanDto>>();
        }
    }
}
