using BiDegree.Api.Features.Example.Endpoints;
using BiDegree.Api.Features.Pictures;
using Microsoft.AspNetCore.Hosting.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.Map("/api", apiApp =>
{
    app.MapPictures();

    //// Configure your API routes here
    //apiApp.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapPictures();

    //});

});

app.MapWeatherForecastEXAMPLE();


app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();

