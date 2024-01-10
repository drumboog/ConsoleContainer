using System.Collections.ObjectModel;

namespace ConsoleContainer.Wpf.ViewModels
{
    internal class ProcessContainerVM : ViewModel
    {
        public ObservableCollection<ProcessVM> Processes { get; } = new ObservableCollection<ProcessVM>();

        public ProcessContainerVM()
        {
            Processes.Add(new ProcessVM("Zookeeper", new ProcessInformation(@"C:\kafka_2.13-3.6.0\bin\windows\zookeeper-server-start.bat")
            {
                Arguments = @"C:\kafka_2.13-3.6.0\config\zookeeper.properties"
            }));
            Processes.Add(new ProcessVM("Kafka Server", new ProcessInformation(@"C:\kafka_2.13-3.6.0\bin\windows\kafka-server-start.bat")
            {
                Arguments = @"C:\kafka_2.13-3.6.0\config\server.properties"
            }));
        }
    }
}
