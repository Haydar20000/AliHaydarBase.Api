using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.DTOs.Response.Members;
using AutoMapper;

namespace AliHaydarBase.Api.Core.Mapper.Members
{


    public class IdCardMappingProfile : Profile
    {
        public IdCardMappingProfile()
        {
            // Member → MemberRowDto
            CreateMap<Member, MemberRowDto>();

            // Member → MemberDetailsDto
            CreateMap<Member, MemberDetailsDto>();

            // PrintHistory → PrintHistoryRecord
            CreateMap<PrintHistory, PrintHistoryRecord>();

            // IdCardTemplate → IdCardTemplateDto
            CreateMap<IdCardTemplate, IdCardTemplateDto>()
                .ForMember(dest => dest.FrontImage,
                    opt => opt.MapFrom(src =>
                        src.FrontImage != null ? Convert.ToBase64String(src.FrontImage) : null))
                .ForMember(dest => dest.BackImage,
                    opt => opt.MapFrom(src =>
                        src.BackImage != null ? Convert.ToBase64String(src.BackImage) : null));
        }
    }

}