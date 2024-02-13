using ConsoleContainer.ProcessManagement.Events;
using System.Diagnostics;
using System.Management;

namespace ConsoleContainer.ProcessManagement
{
    public class ProcessWrapper<TKey> : IProcessWrapper<TKey>
    {
        private readonly object lockTarget = new();

        private Process? process;

        public event EventHandler<ProcessOutputDataEventArgs<TKey>>? OutputDataReceived;

        public event EventHandler<ProcessStateChangedEventArgs<TKey>>? StateChanged;

        public TKey Key { get; }

        public int? ProcessId => process?.Id;

        public ProcessDetails ProcessDetails { get; private set; }

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
                OnStateChanged(new ProcessStateChangedEventArgs<TKey>(Key, value, process?.Id));
            }
        }

        private readonly List<ProcessOutputData> outputData = new List<ProcessOutputData>();
        public IReadOnlyCollection<ProcessOutputData> OutputData => outputData;

        public ProcessWrapper(TKey key, ProcessDetails processDetails)
        {
            Key = key;
            ProcessDetails = processDetails;
        }

        public Task<bool> StartProcessAsync()
        {
            lock (lockTarget)
            {
                try
                {
                    if (process is not null)
                    {
                        return Task.FromResult(false);
                    }

                    State = ProcessState.Starting;

                    var p = new Process();
                    p.StartInfo.FileName = ProcessDetails.FilePath;
                    p.StartInfo.WorkingDirectory = ProcessDetails.WorkingDirectory;
                    p.StartInfo.Arguments = ProcessDetails.Arguments;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.OutputDataReceived += Process_OutputDataReceived;
                    p.ErrorDataReceived += Process_ErrorDataReceived;
                    p.EnableRaisingEvents = true;
                    p.Exited += Process_Exited;
                    p.Start();
                    ChildProcessTracker.AddProcess(p);

                    process = p;

                    State = ProcessState.Running;

                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                }
                catch
                {
                    State = ProcessState.Idle;
                    throw;
                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> StopProcessAsync()
        {
            lock (lockTarget)
            {
                if (process is null)
                {
                    return Task.FromResult(false);
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

            return Task.FromResult(true);
        }

        public Task UpdateProcessDetails(ProcessDetails processDetails)
        {
            lock(lockTarget)
            {
                if (process is not null)
                {
                    throw new Exception("Process cannot be updated if it is running");
                }

                ProcessDetails = processDetails;
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
            OnOutputDataReceived(new ProcessOutputDataEventArgs<TKey>(Key, ProcessDetails, outputData));
        }

        protected virtual void OnOutputDataReceived(ProcessOutputDataEventArgs<TKey> e)
        {
            OutputDataReceived?.Invoke(this, e);
        }

        protected virtual void OnStateChanged(ProcessStateChangedEventArgs<TKey> e)
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
