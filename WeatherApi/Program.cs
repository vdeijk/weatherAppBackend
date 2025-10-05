using WeatherApi.Services;
using WeatherApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(WeatherApi.Controllers.FestivalController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register custom services
builder.Services.AddHttpClient<INasaWeatherService, NasaWeatherService>()
    .AddTypedClient((httpClient, sp) =>
    {
        var service = new NasaWeatherService(httpClient);
        service.UseMockData = true; // NASA api was offline during testing so this is a fallback
        return service;
    });
builder.Services.AddScoped<IFestivalService, FestivalService>();
builder.Services.AddScoped<IWeatherMappingService, WeatherMappingService>();

// Add CORS policy for React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("https://vdeijk.github.io")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseExceptionHandler("/error");

app.UseAuthorization();
app.MapControllers();
app.Run();
