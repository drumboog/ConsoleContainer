using ConsoleContainer.Wpf.Domain;
using System.Diagnostics;
using System.Windows.Media;

namespace ConsoleContainer.Wpf.ViewModels
{
    internal class ProcessVM : ViewModel
    {
        private Process? process;

        public string DisplayName { get; }

        public ProcessInformation ProcessInformation { get; }

        public ProcessOutputVM Output { get; } = new ProcessOutputVM();

        public int? ProcessId
        {
            get => GetProperty<int?>();
            private set => SetProperty(value);
        }

        public bool IsRunning
        {
            get => GetProperty<bool>();
            private set => SetProperty(value, [nameof(CanStart), nameof(CanStop)]);
        }

        public bool CanStart => !IsRunning;

        public bool CanStop => IsRunning;

        public ProcessVM(string displayName, ProcessInformation processInformation)
        {
            DisplayName = displayName;
            ProcessInformation = processInformation;
        }

        public void StartProcess()
        {
            process = new Process();
            process.StartInfo.FileName = ProcessInformation.FileName;
            process.StartInfo.WorkingDirectory = ProcessInformation.WorkingDirectory;
            process.StartInfo.Arguments = ProcessInformation.Arguments;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.Start();
            ChildProcessTracker.AddProcess(process);
            Output.AddOutput("Process started", Brushes.Green);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            ProcessId = process.Id;
            IsRunning = true;
        }

        public void StopProcess()
        {
            var p = process;
            if (p is not null)
            {
                p.CancelOutputRead();
                p.CancelErrorRead();
                if (!p.CloseMainWindow())
                {
                    process?.Kill(true);
                }
            }
            ProcessId = null;
            IsRunning = false;
            Output.AddOutput("Process ended", Brushes.Red);
        }

        public void ClearOutput()
        {
            Output.ClearLogs();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output.AddOutput(e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output.AddOutput(e.Data, Brushes.DarkRed);
        }
    }
}
