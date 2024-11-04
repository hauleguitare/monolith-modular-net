using MonolithModularNET.Auth;
using MonolithModularNET.Master.Infrastructure;
using MonolithModularNET.User.Infrastructure;
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
builder.Services.AddMasterBootstrapper(builder.Configuration, builder.Environment);

// Add SpaStaticFiles
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddSpaStaticFiles(conf =>
    {
        conf.RootPath = "ClientApp/dist";
    });
}



var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

// ADd Static files
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    // Add SPA static files
    app.UseSpaStaticFiles();
}



// Add SPA
app.MapWhen(x => x.Request.Path.Value != null && !x.Request.Path.Value.StartsWith("/api"), conf =>
{
    conf.UseSpa(spaBuilder =>
    {
        spaBuilder.Options.SourcePath = "/ClientApp";
    
        if (builder.Environment.IsDevelopment())
        {
            spaBuilder.UseProxyToSpaDevelopmentServer(new Uri("http://localhost:4200"));
        }
    });
});



// app.MapControllers();

// map api/auth
app.MapMonolithModularNetAuthApi();

// map api/users
app.MapMonolithModularNetUserApi();

// map api/roles
app.MapMonolithModularNetRoleApi();

// map api/master
app.MapMonolithModularNetMaster();

app.Run();