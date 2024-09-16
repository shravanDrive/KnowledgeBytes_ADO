using ADOConnection.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Mappers
{
    public class MappingProfile : Profile
    {
        // Auto Mapper FUnctionality
        // Whatever you wanna map - you can keep it over here.
        public MappingProfile() 
        {
            // Creating a mapping for ignore
            CreateMap<WorkflowDTO, WorkflowModel>()
                .ForMember(dest => dest.IgnoreProperty, opt => opt.Ignore());

            // Creating a mapping for responseDTO vs responseModel
            // where the two have different names
            CreateMap<ResponseDTO, ResponseModel>()
                .ForMember(dest => dest.ModelQuestionId, opt => opt.MapFrom(src => src.DTOQuestionId)); 



        }
    }
}
