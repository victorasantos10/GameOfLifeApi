using Microsoft.EntityFrameworkCore;
using GameOfLifeApi.Data;
using GameOfLifeApi.Services;
using GameOfLifeApi.Repositories;
using GameOfLifeApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
