using System.Diagnostics;
using System.Management;
using ConsoleContainer.ProcessManagement.Events;

namespace ConsoleContainer.ProcessManagement
{
    public class ProcessWrapper : IProcessWrapper
    {
        private readonly object lockTarget = new();

        private Process? process;

        public event EventHandler<ProcessOutputDataEventArgs>? OutputDataReceived;

        public event EventHandler<ProcessStateChangedEventArgs>? StateChanged;

        public int? ProcessId => process?.Id;

        public Guid ProcessLocator => processDetails.ProcessLocator;

        private ProcessDetails processDetails;
        public ProcessDetails ProcessDetails => processDetails;

        private ProcessState state;
        public ProcessState State
        {
            get => state;
            private set
            {
                if (state == value)
                {
                    return;
                }
                state = value;
                OnStateChanged(new ProcessStateChangedEventArgs(value, process?.Id));
            }
        }

        private readonly List<ProcessOutputData> outputData = new List<ProcessOutputData>();
        public IReadOnlyCollection<ProcessOutputData> OutputData => outputData;

        public ProcessWrapper(ProcessDetails processDetails)
        {
            this.processDetails = processDetails;
        }

        public Task StartProcessAsync()
        {
            lock (lockTarget)
            {
                if (process is not null)
                {
                    return Task.CompletedTask;
                }

                State = ProcessState.Starting;

                process = new Process();
                process.StartInfo.FileName = ProcessDetails.FilePath;
                process.StartInfo.WorkingDirectory = ProcessDetails.WorkingDirectory;
                process.StartInfo.Arguments = ProcessDetails.Arguments;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.EnableRaisingEvents = true;
                process.Exited += Process_Exited;
                process.Start();
                ChildProcessTracker.AddProcess(process);

                State = ProcessState.Running;

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            return Task.CompletedTask;
        }

        public Task StopProcessAsync()
        {
            lock (lockTarget)
            {
                if (process is null)
                {
                    return Task.CompletedTask;
                }

                State = ProcessState.Stopping;

                process.CancelOutputRead();
                process.CancelErrorRead();
                if (!process.CloseMainWindow())
                {
                    KillProcessAndChildren(process.Id);
                }

                process = null;

                State = ProcessState.Idle;
            }

            return Task.CompletedTask;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddOutputData(new ProcessOutputData(e.Data));
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddOutputData(new ProcessOutputData(e.Data, true));
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            _ = StopProcessAsync();
        }

        private void AddOutputData(ProcessOutputData outputData)
        {
            this.outputData.Add(outputData);
            OnOutputDataReceived(new ProcessOutputDataEventArgs(ProcessDetails, outputData));
        }

        protected virtual void OnOutputDataReceived(ProcessOutputDataEventArgs e)
        {
            OutputDataReceived?.Invoke(this, e);
        }

        protected virtual void OnStateChanged(ProcessStateChangedEventArgs e)
        {
            StateChanged?.Invoke(this, e);
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
