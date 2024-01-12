using ConsoleContainer.Wpf.Domain;
using ConsoleContainer.Wpf.Exceptions;
using ConsoleContainer.Wpf.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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

        public void AddGroup()
        {
            ProcessGroups.Add(new SettingsProcessGroupVM());
        }

        public void RemoveGroup(SettingsProcessGroupVM group)
        {
            ProcessGroups.Remove(group);
        }

        public void Load()
        {
            try
            {
                ErrorMessages.Clear();

                var repo = new ProcessGroupCollectionRepository();
                var processGroups = repo.Read();

                ProcessGroups.Clear();

                foreach (var g in processGroups.ProcessGroups)
                {
                    var group = new SettingsProcessGroupVM()
                    {
                        GroupName = g.GroupName
                    };
                    foreach(var p in g.Processes)
                    {
                        group.Processes.Add(new SettingsProcessInformationVM()
                        {
                            ProcessName = p.ProcessName,
                            FileName = p.FileName,
                            Arguments = p.Arguments,
                            WorkingDirectory = p.WorkingDirectory
                        });
                    }
                    ProcessGroups.Add(group);
                }
            }
            catch (Exception ex)
            {
                ErrorMessages.Add(ex.Message);
            }
        }

        public void Save()
        {
            try
            {
                ErrorMessages.Clear();

                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true))
                {
                    validationResults.ForEach(x => ErrorMessages.Add(x.ErrorMessage ?? ""));
                    return;
                }

                var collection = new ProcessGroupCollection();
                collection.Update(ProcessGroups);

                var repo = new ProcessGroupCollectionRepository();
                repo.Save(collection);
            }
            catch (Exceptions.ValidationException ex)
            {
                ErrorMessages.Add(ex.ValidationError);
            }
            catch (Exception ex)
            {
                ErrorMessages.Add(ex.Message);
            }
        }
    }
}
