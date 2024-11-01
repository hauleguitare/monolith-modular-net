using MonolithModularNET.Auth;
using WebApi.Bootstraps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen();

// Add MonolithModularNET Authorization Bootstrapper
builder.Services.AddAuthBootstrapper(builder.Configuration, builder.Environment);
builder.Services.AddUserBootstrapper(builder.Configuration, builder.Environment);

// Add SpaStaticFiles
builder.Services.AddSpaStaticFiles(conf =>
{
    conf.RootPath = "ClientApp/dist";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

// ADd Static files
app.UseStaticFiles();

// Add SPA static files
app.UseSpaStaticFiles();

// Add SPA
app.MapWhen(x => x.Request.Path.Value != null && !x.Request.Path.Value.StartsWith("/api"), conf =>
{
    conf.UseSpa(spaBuilder =>
    {
        spaBuilder.Options.SourcePath = "ClientApp";
    
        if (builder.Environment.IsDevelopment())
        {
            spaBuilder.UseProxyToSpaDevelopmentServer(new Uri("http://localhost:4200"));
        }
    });
});



// app.MapControllers();
app.MapMonolithModularNetAuthApi();

app.Run();