

namespace AliHaydarBase.Api.Core.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; }              // unique identifier
        public string? UserId { get; set; }        // who performed the action
        public string? Action { get; set; }        // e.g. "Login", "Logout", "ResetPassword"
        public string? Description { get; set; }   // human-readable summary
        public string? IpAddress { get; set; }    // captured from HttpContext
        public string? UserAgent { get; set; }    // captured from request headers
        public DateTime Timestamp { get; set; }   // when it happened
        public string? DeviceId { get; set; }     // optional, for device-bound actions
        public string? Metadata { get; set; }     // optional JSON for extra details
    }
}
