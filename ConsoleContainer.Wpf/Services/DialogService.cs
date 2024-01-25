using ConsoleContainer.Wpf.Controls.Dialogs;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.ViewModels.Dialogs;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.Services
{
    public class DialogService : IDialogService
    {
        public static DialogService Instance { get; } = new DialogService();

        private DialogService() { }

        public async Task<EditProcessGroupVM?> CreateProcessGroupAsync()
        {
            var vm = new EditProcessGroupVM();
            if (!await ShowDialogAsync<EditProcessGroupControl, bool>(new EditProcessGroupControl() { ViewModel = vm }))
            {
                return null;
            }

            return vm;
        }

        public Task<bool> EditProcessGroupAsync(EditProcessGroupVM processGroup)
        {
            return ShowDialogAsync<EditProcessGroupControl, bool>(new EditProcessGroupControl() { ViewModel = processGroup });
        }

        public async Task<EditProcessVM?> CreateProcessAsync()
        {
            var vm = new EditProcessVM();
            if (!await ShowDialogAsync<EditProcessControl, bool>(new EditProcessControl() { ViewModel = vm }))
            {
                return null;
            }

            return vm;
        }

        public Task<bool> EditProcessAsync(EditProcessVM process)
        {
            return ShowDialogAsync<EditProcessControl, bool>(new EditProcessControl() { ViewModel = process });
        }

        public Task<bool> ShowConfirmationDialogAsync(ConfirmationDialogVM confirmationDialog)
        {
            return ShowDialogAsync<ConfirmationControl, bool>(new ConfirmationControl() { ViewModel = confirmationDialog });
        }

        private Task<TResult> ShowDialogAsync<TDialogControl, TResult>(TDialogControl control)
            where TDialogControl : UserControl, IDialogControl<TResult>
        {
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

            var closeActions = new List<Action<DialogClosedEventArgs<TResult>>>();

            EventHandler<DialogClosedEventArgs<TResult>> closeHandler = (object? sender, DialogClosedEventArgs<TResult> e) =>
            {
                closeActions.ForEach(a => a(e));
            };

            var showDialogEvent = new ShowDialogEvent(control);

            closeActions.Add(result => showDialogEvent.NotifyDialogClosed());
            closeActions.Add(result => taskCompletionSource.SetResult(result.CloseContext));
            closeActions.Add(result => control.DialogClosed -= closeHandler);

            control.DialogClosed += closeHandler;

            _ = EventAggregator.Instance.PublishOnUIThreadAsync(showDialogEvent);

            return taskCompletionSource.Task;
        }
    }
}
