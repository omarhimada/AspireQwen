var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<ONNXGenAIOptions>(
    builder.Configuration.GetSection(ONNXGenAIOptions.SectionName));

builder.Services.AddSingleton<IChatService, ONNXChatService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configured Swagger to show the "Authorize" button for your long API key validation
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "ONNX API",
        Description = "GPT-OSS 20B (CUDA)",
        Version = "v1"
    });

    //deepseek-r1-distill-qwen-14b
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-Key",
        In = ParameterLocation.Header,
        Description = "API key required. e.g.: eyJhbGciOiJIUzI1N..."
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement {
        [new OpenApiSecuritySchemeReference("ApiKey", document)] = []
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => {
    options.EnablePersistAuthorization();
});

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
