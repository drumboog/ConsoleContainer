using ConsoleContainer.Wpf.Domain;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ConsoleContainer.Wpf.ViewModels.Settings
{
    internal class SettingsVM : ViewModel
    {
        public ObservableCollection<SettingsProcessGroupVM> ProcessGroups { get; } = new();

        public ObservableCollection<string> ErrorMessages { get; } = new();

        public bool CanUpdateSettings
        {
            get => GetProperty(false);
            set => SetProperty(value);
        }

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

                VerifyEditability();

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

                VerifyEditability();

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

                _ = EventAggregator.Instance.PublishOnCurrentThreadAsync(new SettingsSavedEvent());
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

        private void VerifyEditability()
        {
            if (AreProcessesRunning())
            {
                CanUpdateSettings = false;
                ErrorMessages.Add("Settings cannot be updated while processes are running.");
            }
            else
            {
                CanUpdateSettings = true;
            }
        }

        private bool AreProcessesRunning()
        {
            //return ProcessContainerVM.Instance.ProcessGroups.SelectMany(x => x.Processes).Any(x => x.IsRunning);
            return true;
        }
    }
}
