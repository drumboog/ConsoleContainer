using ConsoleContainer.ProcessManagement;
using ConsoleContainer.ProcessManagement.Events;
using ConsoleContainer.Wpf.Domain;
using System.Windows.Media;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessVM : ViewModel
    {
        private IProcessWrapper process;

        public string DisplayName => ProcessInformation.ProcessName;

        public ProcessInformation ProcessInformation
        {
            get => GetProperty<ProcessInformation>()!;
            private set => SetProperty(value, [nameof(DisplayName)]);
        }

        public ProcessOutputVM Output { get; } = new ProcessOutputVM();

        public int? ProcessId => process.ProcessId;

        public ProcessState State => process.State;

        public bool IsRunning => State is ProcessState.Starting or ProcessState.Running or ProcessState.Stopping;

        public bool CanStart => !IsRunning;

        public bool CanStop => IsRunning;

        public ProcessVM(ProcessInformation processInformation)
        {
            ProcessInformation = processInformation;
            process = CreateProcessWrapper(processInformation);
        }

        internal ProcessVM(string processName, IProcessWrapper processWrapper)
        {
            process = processWrapper;
            ProcessInformation = new ProcessInformation(processName, processWrapper.ProcessDetails.FilePath, processWrapper.ProcessDetails.Arguments, process.ProcessDetails.WorkingDirectory);
        }

        public void Update(ProcessInformation processInformation)
        {
            if (process is not null && process.State is ProcessState.Idle)
            {
                throw new Exception("Can't update process while it is running");
            }

            ProcessInformation = processInformation;
            process = CreateProcessWrapper(processInformation);
        }

        public void StartProcess()
        {
            if (IsRunning)
            {
                return;
            }

            process.StartProcess();
        }

        public void StopProcess()
        {
            process.StopProcess();
        }

        public void ClearOutput()
        {
            Output.ClearLogs();
        }

        private ProcessWrapper CreateProcessWrapper(ProcessInformation processInformation)
        {
            var details = new ProcessDetails(Guid.NewGuid().ToString(), processInformation.FilePath, processInformation.Arguments, processInformation.WorkingDirectory);
            var result = new ProcessWrapper(details);
            result.OutputDataReceived += Result_OutputDataReceived;
            result.StateChanged += Result_StateChanged;
            return result;
        }

        private void Result_OutputDataReceived(object? sender, ProcessOutputDataEventArgs e)
        {
            Output.AddOutput(e.Data.Data, e.Data.IsProcessError ? Brushes.DarkRed : null);
        }

        private void Result_StateChanged(object? sender, ProcessStateChangedEventArgs e)
        {
            switch (e.State)
            {
                case ProcessState.Idle:
                    Output.AddOutput("Process stopped", Brushes.Red);
                    break;
                case ProcessState.Starting:
                    Output.AddOutput("Process starting", Brushes.Green);
                    break;
                case ProcessState.Running:
                    Output.AddOutput("Process started", Brushes.LightGreen);
                    break;
                case ProcessState.Stopping:
                    Output.AddOutput("Stopping process", Brushes.IndianRed);
                    break;
            }

            OnPropertyChanged(nameof(ProcessId));
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(IsRunning));
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanStop));
        }
    }
}
