namespace ConsoleContainer.Contracts
{
    public interface IProcessHub
    {
        Task AddProcess(NewProcessDto process);
    }
}
