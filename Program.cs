using AliHaydarBase.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Modular DI with environment
builder.Services.AddAliHaydarBaseServices(builder.Configuration, builder.Environment);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowWebClient");
app.UseAuthentication();
app.UseAuthorization();

// Minimal API endpoints go here
app.MapAuthEndpoints();

app.Run();