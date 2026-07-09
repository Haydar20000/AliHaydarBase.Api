using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Core.Models.MonitoringData;
using AliHaydarBase.Api.DTOs.Request.MonitoringDataDtos;
using AliHaydarBase.Api.DTOs.Request.Template;
using AliHaydarBase.Api.DTOs.Response.Members;
using AliHaydarBase.Api.HelperFunctions;
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
            CreateMap<Member, MemberRowDto>()
                .ForMember(d => d.FullNameArabic, o => o.MapFrom(s => s.FullNameArabic))
                .ForMember(d => d.FullNameEnglish, o => o.MapFrom(s => s.FullNameEnglish))
                .ForMember(d => d.Stage, o => o.MapFrom(s => s.Stage))
                .ForMember(d => d.RegisterDate, o => o.MapFrom(s => s.RegisterDate))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth))
                .ForMember(d => d.LastYearIdentityRenewal, o => o.MapFrom(s => s.LastYearIdentityRenewal))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.IsBlockedByAdmin ? "Blocked" : "Active"))
                .ForMember(d => d.ImageBase64, o => o.MapFrom(s => ImageHelper.ConvertImageUrlToBase64(s.ImageUrl)))
                .ForMember(d => d.IsPrinted, o => o.MapFrom(s => s.IsIdPrinted))
                .ForMember(d => d.IsBlockedByAdmin, o => o.MapFrom(s => s.IsBlockedByAdmin));


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

            CreateMap<PrintHistoryRecord, PrintHistoryRowDto>()
                 .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member.FullNameArabic))
                 .ForMember(d => d.TemplateName, o => o.MapFrom(s => s.Template.TemplateName))
                 .ForMember(d => d.FrontThumbnailBase64, o => o.MapFrom(s => s.FrontThumbnailBase64))
                 .ForMember(d => d.RegisterNumber, o => o.MapFrom(s => s.Member.RegisterNumber))
                 .ForMember(d => d.BackThumbnailBase64, o => o.MapFrom(s => s.BackThumbnailBase64));
            // TemplateVersion → TemplateVersionDto
            CreateMap<TemplateVersion, TemplateVersionDto>().ReverseMap();


        }
    }

}