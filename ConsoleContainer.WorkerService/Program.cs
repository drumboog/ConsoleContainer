using ConsoleContainer.ProcessManagement;
using ConsoleContainer.Repositories;
using ConsoleContainer.WorkerService;
using ConsoleContainer.WorkerService.Hubs;
using NReco.Logging.File;

var builder = WebApplication.CreateBuilder(args);

var loggingRootPath = Path.Join(builder.Environment.ContentRootPath, "Logs");
var logFile = Path.Join(loggingRootPath, "app.log");

//Directory.CreateDirectory(loggingRootPath);
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
