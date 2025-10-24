using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response
{
    public class ClaimDefinitionResponseDto
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string UiHint { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public bool IsVisibleToFrontend { get; set; }

    }
}