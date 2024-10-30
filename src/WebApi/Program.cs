using MonolithModularNET.Auth;
using WebApi.Bootstraps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen();

// Add MonolithModularNET Auth Bootstrapper
builder.Services.AddAuthBootstrapper(builder.Configuration, builder.Environment);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

// app.MapControllers();
app.MapMonolithModularNetAuthApi();


app.Run();