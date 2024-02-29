using ConsoleContainer.ProcessManagement.Events;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Management;

namespace ConsoleContainer.ProcessManagement
{
    public class ProcessWrapper<TKey> : IProcessWrapper<TKey>
    {
        private readonly object lockTarget = new();
        private readonly ILogger<ProcessWrapper<TKey>> logger;
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

        public ProcessWrapper(ILogger<ProcessWrapper<TKey>> logger, TKey key, ProcessDetails processDetails)
        {
            this.logger = logger;
            Key = key;
            ProcessDetails = processDetails;
        }

        public Task<bool> StartProcessAsync()
        {
            logger.LogInformation($"{Key} - Waiting to start process");
            lock (lockTarget)
            {
                try
                {
                    if (process is not null)
                    {
                        logger.LogInformation($"{Key} - Process is not null.  Start not needed.");
                        return Task.FromResult(false);
                    }

                    logger.LogInformation($"{Key} - Changing state to Starting");
                    State = ProcessState.Starting;
                    logger.LogInformation($"{Key} - State changed to Starting");

                    logger.LogInformation($"{Key} - Configuring new process");
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

                    logger.LogInformation($"{Key} - Starting process");
                    p.Start();
                    logger.LogInformation($"{Key} - Tracking process as child");
                    ChildProcessTracker.AddProcess(p);
                    logger.LogInformation($"{Key} - Tracking initialized");

                    process = p;

                    logger.LogInformation($"{Key} - Changing state to Running");
                    State = ProcessState.Running;
                    logger.LogInformation($"{Key} - State changed to Running");

                    logger.LogInformation($"{Key} - Beginning to read output and error streams");
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    logger.LogInformation($"{Key} - Streams initialized");
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"{Key} - Changing state to Idle");
                    State = ProcessState.Idle;
                    logger.LogInformation($"{Key} - State changed to Idle");
                    process = null;
                    logger.LogError(ex, $"{Key} - An error occurred starting the process");
                    throw;
                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> StopProcessAsync()
        {
            logger.LogInformation($"{Key} - Waiting to stop process");
            lock (lockTarget)
            {
                if (process is null)
                {
                    logger.LogInformation($"{Key} - Process is null.  Stop not needed.");
                    return Task.FromResult(false);
                }

                logger.LogInformation($"{Key} - Changing state to Stopping");
                State = ProcessState.Stopping;
                logger.LogInformation($"{Key} - State changed to Stopping");

                logger.LogInformation($"{Key} - Cancelling output and error streams");
                process.CancelOutputRead();
                process.CancelErrorRead();

                logger.LogInformation($"{Key} - Closing main window");
                if (!process.CloseMainWindow())
                {
                    logger.LogInformation($"{Key} - Main window failed to close, killing processes and children");
                    KillProcessAndChildren(process.Id);
                }

                process = null;

                logger.LogInformation($"{Key} - Changing state to Idle");
                State = ProcessState.Idle;
                logger.LogInformation($"{Key} - State changed to Idle");
            }

            return Task.FromResult(true);
        }

        public Task UpdateProcessDetails(ProcessDetails processDetails)
        {
            logger.LogInformation($"{Key} - Waiting to update process details");
            lock (lockTarget)
            {
                logger.LogInformation($"{Key} - Updating process details");
                if (process is not null)
                {
                    logger.LogInformation($"{Key} - Process exists, throwing exception");
                    throw new Exception("Process cannot be updated if it is running");
                }

                ProcessDetails = processDetails;
                logger.LogInformation($"{Key} - Process details updated");
            }

            return Task.CompletedTask;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            logger.LogInformation($"{Key} - Output data received - {e.Data}");
            AddOutputData(new ProcessOutputData(e.Data));
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            logger.LogInformation($"{Key} - Error data received - {e.Data}");
            AddOutputData(new ProcessOutputData(e.Data, true));
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            logger.LogInformation($"{Key} - Process exited");
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
