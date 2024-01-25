using ConsoleContainer.Wpf.Domain;
using ConsoleContainer.Wpf.ViewModels;

namespace ConsoleContainer.Wpf.DesignData
{
    internal class ViewModelLocator
    {
        public static ProcessContainerVM ProcessContainer { get; } = CreateProcessContainer();

        public static ProcessVM Process { get; } = new ProcessVM(new ProcessInformation("Application UI", @"C:\Application\FE\ApplicationFE.exe", @"", @"C:\Application\FE\"), 12345, true);


        private static ProcessContainerVM CreateProcessContainer()
        {
            var result = new ProcessContainerVM();
            result.ProcessGroups.Add(
                CreateProcessGroup(
                    "Application Processes",
                    new ProcessVM(new ProcessInformation("Application UI", @"C:\Application\FE\ApplicationFE.exe", @"", @"C:\Application\FE\"), 15434, true),
                    new ProcessVM(new ProcessInformation("Application API", @"C:\Application\API\ApiService.exe", @"", @"C:\Application\API\"), 65493, false),
                    new ProcessVM(new ProcessInformation("Application BFF", @"C:\Application\BFF\BffService.exe", @"", @"C:\Application\BFF\"), 93284, false),
                    new ProcessVM(new ProcessInformation("Application Database", @"C:\Application\DB\DatabaseService.exe", @"", @"C:\Application\DB\"), 20938, true)
                )
            );
            result.ProcessGroups.Add(
                CreateProcessGroup(
                    "Windows Processes"
                )
            );
            result.ProcessGroups.Add(
                CreateProcessGroup(
                    "Other Processes"
                )
            );
            return result;
        }

        private static ProcessGroupVM CreateProcessGroup(string groupName, params ProcessVM[] processes)
        {
            var result = new ProcessGroupVM();
            result.GroupName = groupName;
            foreach (var p in processes)
            {
                result.Processes.Add(p);
            }
            return result;
        }
    }
}
