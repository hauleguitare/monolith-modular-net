using MonolithModularNET.Auth;
using WebApi.Bootstraps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add MonolithModularNET Cache Bootstrapper
builder.Services.AddCacheBootstrapper(builder.Configuration, builder.Environment);

// Add MonolithModularNET Auth Bootstrapper
builder.Services.AddAuthBootstrapper(builder.Configuration, builder.Environment);

// Add Authentication
builder.Services.AddResourceAuthentication(builder.Configuration, builder.Environment);

// Add Authorization
builder.Services.AddResourceAuthorization(builder.Configuration, builder.Environment);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapGet("api/hello-world", () => "Hello World").RequireAuthorization();
}

app.UseAuthentication();

app.UseAuthorization();

// app.MapControllers();
app.MapMonolithModularNetAuthApi();


app.Run();