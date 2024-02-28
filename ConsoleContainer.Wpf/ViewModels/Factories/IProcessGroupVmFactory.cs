using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.ViewModels.Factories
{
    public interface IProcessGroupVmFactory
    {
        ProcessGroupVM Create(Guid processGroupId, string groupName);
    }
}
