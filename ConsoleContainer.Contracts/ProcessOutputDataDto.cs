namespace ConsoleContainer.Contracts
{
    public class ProcessOutputDataDto
    {
        public Guid ProcessGroupId { get; set; }
        public Guid ProcessLocator { get; set; }
        public string? Data { get; set; }
        public bool IsProcessError { get; set; }
    }
}
