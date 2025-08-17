using CommandService.AsyncDataServices;
using CommandService.Data;
using CommandService.EventProcessing;
using CommandService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

PrepDb.PrepPopulation(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

