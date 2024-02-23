using ConsoleContainer.ProcessManagement;
using ConsoleContainer.Repositories;
using ConsoleContainer.Repositories.Configuration;
using ConsoleContainer.WorkerService;
using ConsoleContainer.WorkerService.Configuration;
using ConsoleContainer.WorkerService.Hubs;
using NReco.Logging.File;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var applicationSettings = builder.Configuration.GetRequiredValue<ApplicationSettings>("ApplicationSettings");

var applicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), applicationSettings.ApplicationDataDirectoryName);
var loggingRootPath = Path.Join(applicationDataDirectory, "Logs");
var logFile = Path.Join(loggingRootPath, "workerService.log");

builder.Logging.AddFile(logFile, options =>
{
    options.Append = true;
    options.MaxRollingFiles = 100;
    options.FileSizeLimitBytes = 1024 * 1024 * 10; // 10MB
});
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddWorkerService();
builder.Services.AddProcessManagement();
builder.Services.AddRepositories(options =>
{
    options.WithRootDirectory(applicationDataDirectory);
});
builder.Services.AddSignalR()
    .AddMessagePackProtocol();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddWindowsService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHub<ProcessHub>("/signalr/Process");

app.Run();
