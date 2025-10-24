using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models
{
    public class ClaimDefinition
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Core identity
        public string Type { get; set; } = string.Empty; // e.g. "Department", "CanEditGrades"
        public string Description { get; set; } = string.Empty; // Human-readable explanation
        public string Category { get; set; } = string.Empty; // e.g. "Access", "Permission", "UI"

        // UI and frontend hints
        public string UiHint { get; set; } = string.Empty; // e.g. "ShowGradeEditor", "EnableExportButton"
        public bool IsVisibleToFrontend { get; set; } = true; // Controls exposure in Flutter

        // Scope and grouping
        public string Scope { get; set; } = string.Empty; // e.g. "StudentManagement", "FinanceTools"
        public string Group { get; set; } = string.Empty; // e.g. "Academic", "Administrative"

        // Lifecycle and control
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}