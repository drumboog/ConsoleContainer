namespace ConsoleContainer.WorkerService
{
    public struct ProcessKey
    {
        public Guid ProcessGroupId { get; set; }
        public Guid ProcessLocator { get; set; }

        public ProcessKey(Guid processGroupId, Guid processLocator)
        {
            ProcessGroupId = processGroupId;
            ProcessLocator = processLocator;
        }

        public override string ToString()
        {
            return $"ProcessGroupId: {ProcessGroupId}, ProcessLocator: {ProcessLocator}";
        }
    }
}
