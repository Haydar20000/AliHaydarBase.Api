using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.Core.Models.MonitoringData;
using AliHaydarBase.Api.DTOs.Request.MonitoringDataDtos;
using AutoMapper;

namespace AliHaydarBase.Api.Core.Mapper.MonitoringData
{
    public class PrintHistoryProfile : Profile
    {
        public PrintHistoryProfile()
        {
            // Entity → Row DTO (list view)
            CreateMap<PrintHistory, PrintHistoryRowDto>();

            // Entity → Details DTO (full view)
            CreateMap<PrintHistory, PrintHistoryDetailsDto>();
        }
    }
}