using Microsoft.OpenApi.Models;
using WebApp.Auth.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
    {
        Description = "Api Key to secure the API",
        Type = SecuritySchemeType.ApiKey,
        Name = AuthConfig.ApiKeyHeader,
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"

    });

    var scheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement()
    {
            { scheme, new List<string>()}
    };

    options.AddSecurityRequirement(requirement);


});

builder.Services.AddScoped<ApiAuthenticationFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
