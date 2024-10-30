using MonolithModularNET.Auth;
using MonolithModularNET.Auth.Core;
using WebApi.Bootstraps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add MonolithModularNET Auth Bootstrapper
builder.Services.AddAuthBootstrapper(builder.Configuration, builder.Environment);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapGet("api/hello-world", [RoleBasedAuthorize("greeting:read")] () => "Hello World");
}

app.UseAuthentication();

app.UseAuthorization();

// app.MapControllers();
app.MapMonolithModularNetAuthApi();


app.Run();