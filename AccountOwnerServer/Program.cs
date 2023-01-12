using AccountOwnerServer.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using NLog;

using Entities;
using System.Text.Json.Serialization;

using Entities.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var cl = new Owner();
Type t = cl.GetType();
//var FieldInfoObject = typeof(T).GetField(FieldName);
var FieldInfoObject = t.GetField("name", BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.ConfigureCors(); 
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
//builder.Services.ConfigureMySqlContext(builder.Configuration);
var connectionString = builder.Configuration.GetConnectionString("connectionString");
builder.Services.AddDbContext<RepositoryContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("connectionString"), b => b.MigrationsAssembly("Entities")));
builder.Services.ConfigureRepositoryWrapper();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer(); //LTPE
builder.Services.AddSwaggerGen(); // LTPE

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // LTPE

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.Use(async (context, next) =>
    {
        await next();
        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
        {
            context.Request.Path = "/index.html"; await next();
        }
    });

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions 
{ 
    ForwardedHeaders = ForwardedHeaders.All 
}); 

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
