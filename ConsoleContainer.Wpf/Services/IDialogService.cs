using ConsoleContainer.Wpf.ViewModels.Dialogs;

namespace ConsoleContainer.Wpf.Services
{
    public interface IDialogService
    {
        Task<EditProcessGroupVM?> CreateProcessGroupAsync();
        Task<bool> EditProcessGroupAsync(EditProcessGroupVM processGroup);

        Task<EditProcessVM?> CreateProcessAsync();
        Task<bool> EditProcessAsync(EditProcessVM process);

        Task<bool> ShowConfirmationDialogAsync(ConfirmationDialogVM confirmationDialog);
    }
}
