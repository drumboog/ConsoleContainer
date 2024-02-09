using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.ViewModels.Factories
{
    public interface IProcessVmFactory
    {
        ProcessVM Create(Guid processGroupId, Guid processLocator, int? processId, string processName, string filePath, string? arguments, string? workingDirectory, ProcessState state);
    }
}
