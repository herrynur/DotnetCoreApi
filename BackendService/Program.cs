using BackendService;
using BackendService.Helper.Middleware;
using BackendService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Add infrastructure services
builder.Services
    .AddCustomDbContext(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddCustomMvc()
    .AddSwaggerGen()
    .AddInfrastructureServices()
    .AddAplicationSettings(builder.Configuration)
    .AddSwaggerOption()
    .AddAuthorization()
    .AddCustomAuthentication(builder.Configuration)
    .AddCustomAuthorization()
    .AddEndpointsApiExplorer()
    .AddMemoryCache()
    .AddHealthChecks();

var app = builder.Build();

//Configure pathbase
var pathBase = builder.Configuration["PathBase"];
var usePathBase = !string.IsNullOrWhiteSpace(pathBase);
if (usePathBase)
{
    app.UsePathBase(pathBase);
}

// Configure the HTTP request pipeline.

// use swagger
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseErrorHandler();

app.MapHealthChecks("/hc");

//seed data
ApplicationContextInitialiser.SeedData(app.Services);

app.Run();
