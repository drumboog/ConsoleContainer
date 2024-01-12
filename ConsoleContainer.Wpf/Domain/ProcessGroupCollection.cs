using ConsoleContainer.Wpf.Domain.Contracts;
using ConsoleContainer.Wpf.Extensions;
using Newtonsoft.Json;

namespace ConsoleContainer.Wpf.Domain
{
    public class ProcessGroupCollection
    {
        [JsonProperty]
        private readonly List<ProcessGroup> processGroups = new();

        [JsonIgnore]
        public IEnumerable<ProcessGroup> ProcessGroups => processGroups;

        public ProcessGroup AddGroup(string groupName)
        {
            var group = new ProcessGroup(groupName);
            processGroups.Add(group);
            return group;
        }

        public void Update(IEnumerable<IProcessGroup> processGroups)
        {
            var groups = processGroups.Select(x => new ProcessGroup(x.GroupName.Required(), CreateProcessInformation(x.Processes)));
            this.processGroups.Clear();
            this.processGroups.AddRange(groups);
        }

        private IEnumerable<ProcessInformation> CreateProcessInformation(IEnumerable<IProcessInformation> pi)
        {
            return pi.Select(x =>
                new ProcessInformation(
                    x.ProcessName.Required(),
                    x.FileName.Required(),
                    x.Arguments,
                    x.WorkingDirectory
                )
            );
        }
    }
}
