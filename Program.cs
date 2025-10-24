using AliHaydarBase.Api.Dependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Services from Dependencies\ServiceCollectionExtensions
builder.Services.AddAliHaydarBaseServices(builder.Configuration);

// Add Swagger services to the container.... Start
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Swagger services to the container.... End

// Allow the app to use controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseWebAssemblyDebugging();
    // Add Swagger services to the container.... Start
    app.UseSwagger();
    app.UseSwaggerUI();
    // Add Swagger services to the container.... End
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowWebClient");

app.MapFallbackToFile("index.html");

app.UseAntiforgery();

app.Run();