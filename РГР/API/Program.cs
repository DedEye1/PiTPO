using API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<SimulationService>();

// Настройка CORS (для разработки, если фронтенд будет на другом порту)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseDefaultFiles();     // ищет index.html в wwwroot
app.UseStaticFiles();      // отдаёт статические файлы
app.MapControllers();

// Fallback для SPA – все не-API запросы отдают index.html
app.MapFallbackToFile("index.html");

app.Run();