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