using MonolithModularNET.Auth;
using WebApi.Bootstraps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add MonolithModularNET Auth Bootstrapper
builder.Services.AddMonolithModularNetAuthBootstrapper(builder.Configuration, builder.Environment);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

// app.MapControllers();
app.MapMonolithModularNetAuthApis();


app.Run();