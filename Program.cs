using AliHaydarBase.Api.Dependencies;
using AliHaydarBase.Api.Endpoints;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// 🧩 Modular DI with environment-specific configuration
builder.Services.AddAliHaydarBaseServices(builder.Configuration, builder.Environment);

// 📘 Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://0.0.0.0:5164");

var app = builder.Build();

// 🛠️ Auto-apply EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AliHaydarDbContext>();
    db.Database.Migrate(); // Applies any pending migrations
}

// 🚦 Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();       // 📘 Swagger UI in dev
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error"); // 🧯 Global error handler
    app.UseHsts();                     // 🔐 Enforce HTTPS in production
}

app.UseHttpsRedirection();       // 🔐 Redirect HTTP to HTTPS
app.UseCors("AllowWebClient");   // 🌐 CORS policy for frontend
app.UseIpRateLimiting();         // 🛡️ Protect sensitive endpoints from abuse
app.UseAuthentication();         // 🔐 Validate JWTs and external logins
app.UseAuthorization();          // ✅ Enforce access policies

// 🚀 Minimal API endpoints
app.MapAuthEndpoints();          // 🔐 Auth routes (login, register, etc.)

app.Run(); // 🏁 Start the application