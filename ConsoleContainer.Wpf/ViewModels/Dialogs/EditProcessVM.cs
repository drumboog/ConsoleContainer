using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ConsoleContainer.Wpf.ViewModels.Dialogs
{
    public class EditProcessVM : ViewModel
    {
        public ObservableCollection<string> ErrorMessages { get; } = new();

        public bool HasErrors => ErrorMessages.Any();

        public EditProcessVM()
        {
            ErrorMessages.CollectionChanged += delegate { OnPropertyChanged(nameof(HasErrors)); };
        }

        [Required]
        public string ProcessName
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        [Required]
        public string FilePath
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        public string? Arguments
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        public string? WorkingDirectory
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        public bool AutoStart
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        public bool RestartOnError
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        public bool RestartOnExit
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        public bool Validate()
        {
            try
            {
                ErrorMessages.Clear();

                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true))
                {
                    validationResults.ForEach(x => ErrorMessages.Add(x.ErrorMessage ?? ""));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorMessages.Add(ex.Message);
                return false;
            }
        }
    }
}
