using Newtonsoft.Json;

namespace ConsoleContainer.Wpf.Domain
{
    public class ProcessGroup
    {
        [JsonProperty]
        private readonly List<ProcessInformation> processes;

        [JsonIgnore]
        public IEnumerable<ProcessInformation> Processes => processes;

        [JsonProperty]
        public string GroupName { get; private set; }

        [JsonConstructor]
        public ProcessGroup(string groupName)
            : this(groupName, Enumerable.Empty<ProcessInformation>())
        {
        }

        public ProcessGroup(string groupName, IEnumerable<ProcessInformation> processes)
        {
            GroupName = groupName;
            this.processes = processes.ToList();
        }

        public void AddProcess(ProcessInformation process)
        {
            processes.Add(process);
        }
    }
}
