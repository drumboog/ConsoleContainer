using ConsoleContainer.Wpf.Domain;
using ConsoleContainer.Wpf.Exceptions;
using ConsoleContainer.Wpf.Repositories;
using System.Collections.ObjectModel;

namespace ConsoleContainer.Wpf.ViewModels.Settings
{
    internal class SettingsVM : ViewModel
    {
        public ObservableCollection<SettingsProcessGroupVM> ProcessGroups { get; } = new();

        public ObservableCollection<string> ErrorMessages { get; } = new();

        public bool HasErrors => ErrorMessages.Any();

        public SettingsVM()
        {
            ErrorMessages.CollectionChanged += delegate { OnPropertyChanged(nameof(HasErrors)); };
        }

        public void MoveGroupUp(SettingsProcessGroupVM group)
        {
            var index = ProcessGroups.IndexOf(group);
            if (index == 0)
            {
                ProcessGroups.Move(index, ProcessGroups.Count - 1);
            }
            else
            {
                ProcessGroups.Move(index, index -1);
            }
        }

        public void MoveGroupDown(SettingsProcessGroupVM group)
        {
            var index = ProcessGroups.IndexOf(group);
            var newIndex = (index + 1) % ProcessGroups.Count;
            ProcessGroups.Move(index, newIndex);
        }

        public void Save()
        {
            try
            {
                ErrorMessages.Clear();

                var collection = new ProcessGroupCollection();
                collection.Update(ProcessGroups);

                var repo = new ProcessGroupCollectionRepository();
                repo.Save(collection);
            }
            catch (ValidationException ex)
            {
                ErrorMessages.Add(ex.Message);
            }
            catch (Exception ex)
            {
                ErrorMessages.Add(ex.Message);
            }
        }
    }
}
