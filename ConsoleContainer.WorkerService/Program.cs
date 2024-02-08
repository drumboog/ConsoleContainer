using ConsoleContainer.ProcessManagement;
using ConsoleContainer.Repositories;
using ConsoleContainer.WorkerService;
using ConsoleContainer.WorkerService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWorkerService();
builder.Services.AddProcessManagement();
builder.Services.AddRepositories();
builder.Services.AddSignalR();

builder.Services.AddControllers();
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
app.MapHub<ProcessHub>("/signalr/Process");

app.Run();
