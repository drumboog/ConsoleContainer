using ConsoleContainer.Wpf.Services;
using ConsoleContainer.Wpf.ViewModels.Dialogs;

namespace ConsoleContainer.Wpf.DesignData
{
    internal partial class ViewModelLocator
    {
        private class MockDialogService : IDialogService
        {
            public Task<EditProcessVM?> CreateProcessAsync()
            {
                throw new NotImplementedException();
            }

            public Task<EditProcessGroupVM?> CreateProcessGroupAsync()
            {
                throw new NotImplementedException();
            }

            public Task<bool> EditProcessAsync(EditProcessVM process)
            {
                throw new NotImplementedException();
            }

            public Task<bool> EditProcessGroupAsync(EditProcessGroupVM processGroup)
            {
                throw new NotImplementedException();
            }

            public Task<bool> ShowConfirmationDialogAsync(ConfirmationDialogVM confirmationDialog)
            {
                throw new NotImplementedException();
            }
        }
    }
}
