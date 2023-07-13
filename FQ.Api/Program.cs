using FQ.Application.Interfaces;
using FQ.Application.Services;
using FQ.Domain.Entities;
using FQ.Infrastructure.AzureService;
using FQ.Infrastructure.Interfaces;
using FQ.Infrastructure.Repositories;
using Google.Api;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PostRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Dentro del método ConfigureServices en Startup.cs
builder.Services.AddScoped<FQ.Application.Validators.Posts.PostValidators>();
builder.Services.AddScoped<IPostsApplication, PostApplication>();
builder.Services.AddScoped<ContentModeratorService>();

//Cors
builder.Services.AddCors(options => {

    options.AddPolicy("Politica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader() 
        .AllowAnyMethod();
    }
    );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Politica");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
