using ConsoleContainer.Wpf.Domain;
using System.Diagnostics;
using System.Management;
using System.Windows.Media;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessVM : ViewModel
    {
        private Process? process;

        public string DisplayName
        {
            get => GetProperty<string>()!;
            private set => SetProperty(value);
        }

        public ProcessInformation ProcessInformation
        {
            get => GetProperty<ProcessInformation>()!;
            private set => SetProperty(value);
        }

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

        public ProcessVM(ProcessInformation processInformation)
        {
            DisplayName = processInformation.ProcessName;
            ProcessInformation = processInformation;
        }

        internal ProcessVM(ProcessInformation processInformation, int? processId, bool isRunning)
            : this(processInformation)
        {
            ProcessId = processId;
            IsRunning = isRunning;
        }

        public void Update(ProcessInformation processInformation)
        {
            if (IsRunning)
            {
                throw new Exception("Can't update process while it is running");
            }

            DisplayName = processInformation.ProcessName;
            ProcessInformation = processInformation;
        }

        public void StartProcess()
        {
            if (IsRunning)
            {
                return;
            }

            process = new Process();
            process.StartInfo.FileName = ProcessInformation.FileName;
            process.StartInfo.WorkingDirectory = ProcessInformation.WorkingDirectory;
            process.StartInfo.Arguments = ProcessInformation.Arguments;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.LoadUserProfile = true;
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
                    KillProcessAndChildren(p.Id);
                }
                Output.AddOutput("Process ended", Brushes.Red);
            }
            ProcessId = null;
            IsRunning = false;
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

        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
    }
}
