using Newtonsoft.Json;

namespace ConsoleContainer.Domain
{
    public class ProcessGroupCollection
    {
        [JsonProperty]
        private readonly List<ProcessGroup> processGroups = new();

        [JsonIgnore]
        public IEnumerable<ProcessGroup> ProcessGroups => processGroups;

        public ProcessGroup? this[Guid groupId] => processGroups.FirstOrDefault(g => g.ProcessGroupId == groupId);

        public ProcessGroup AddGroup(Guid groupId, string groupName)
        {
            if (ProcessGroups.Any(g => g.ProcessGroupId == groupId))
            {
                throw new Exception($"Process already exists with group id {groupId}");
            }
            var group = new ProcessGroup(groupId, groupName);
            processGroups.Add(group);
            return group;
        }

        public bool DeleteGroup(Guid groupId)
        {
            return processGroups.RemoveAll(g => g.ProcessGroupId == groupId) > 0;
        }
    }
}
