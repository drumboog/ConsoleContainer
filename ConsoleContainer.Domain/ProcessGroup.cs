using ConsoleContainer.Domain.Contracts;
using Newtonsoft.Json;

namespace ConsoleContainer.Domain
{
    public class ProcessGroup
    {
        public Guid ProcessGroupId { get; }

        [JsonProperty]
        private readonly List<ProcessInformation> processes;

        [JsonIgnore]
        public IEnumerable<ProcessInformation> Processes => processes;

        [JsonProperty]
        public string GroupName { get; private set; }

        [JsonConstructor]
        public ProcessGroup(Guid processGroupId, string groupName)
            : this(processGroupId, groupName, Enumerable.Empty<ProcessInformation>())
        {
        }

        public ProcessGroup(Guid processGroupId, string groupName, IEnumerable<ProcessInformation> processes)
        {
            ProcessGroupId = processGroupId;
            GroupName = groupName;
            this.processes = processes.ToList();
        }

        public void Update(string groupName)
        {
            GroupName = groupName;
        }

        public ProcessInformation AddProcess(Guid processLocator, IProcessInformation processInformation)
        {
            var info = new ProcessInformation(processLocator, processInformation);
            processes.Add(info);
            return info;
        }

        public ProcessInformation? ReplaceProcess(Guid processLocator, IProcessInformation processInformation)
        {
            var index = processes.FindIndex(p => p.ProcessLocator == processLocator);
            if (index < 0)
            {
                return null;
            }
            var info = new ProcessInformation(processLocator, processInformation);
            processes[index] = info;
            return info;
        }

        public bool DeleteProcess(Guid processLocator)
        {
            return processes.RemoveAll(p => p.ProcessLocator == processLocator) > 0;
        }
    }
}
