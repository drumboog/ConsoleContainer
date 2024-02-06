using ConsoleContainer.Contracts;
using ConsoleContainer.Eventing;
using ConsoleContainer.ProcessManagement;
using ConsoleContainer.WorkerService;
using ConsoleContainer.WorkerService.Hubs;
using ConsoleContainer.WorkerService.HubSubscriptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IProcessHubSubscription, ProcessHubSubscription>();

builder.Services.AddEventing();
builder.Services.AddProcessManagement();
builder.Services.AddSignalR();
builder.Services.AddHostedService<ProcessWorker>();

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
