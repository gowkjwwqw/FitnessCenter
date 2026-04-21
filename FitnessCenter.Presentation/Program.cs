using FitnessCenter.Application.Halls;
using FitnessCenter.Application.Halls.Abstractions;
using FitnessCenter.Application.Sessions;
using FitnessCenter.Application.Sessions.Abstractions;
using FitnessCenter.Application.Trainers;
using FitnessCenter.Application.Trainers.Abstractions;
using FitnessCenter.Infrastructure.Halls;
using FitnessCenter.Infrastructure.Sessions;
using FitnessCenter.Infrastructure.Trainers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITrainersRepository, TrainersRepository>();
builder.Services.AddSingleton<IHallsRepository, HallsRepository>();
builder.Services.AddSingleton<ITrainingSessionsRepository, TrainingSessionsRepository>();

builder.Services.AddScoped<TrainersService>();
builder.Services.AddScoped<HallsService>();
builder.Services.AddScoped<TrainingSessionsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fitness Center API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fitness Center API v1");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.MapControllers();
app.Run();