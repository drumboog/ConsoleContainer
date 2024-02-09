// Source based on https://github.com/Caliburn-Micro/Caliburn.Micro

namespace ConsoleContainer.Wpf.Eventing
{
    /// <summary>
    /// Interface for platform specific operations that need enlightenment.
    /// </summary>
    public interface IPlatformProvider
    {
        /// <summary>
        /// Whether or not classes should execute property change notications on the UI thread.
        /// </summary>
        bool PropertyChangeNotificationsOnUIThread { get; }

        /// <summary>
        ///   Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void BeginOnUIThread(Action action);

        /// <summary>
        ///   Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name = "action">The action to execute.</param>
        Task OnUIThreadAsync(Func<Task> action);

        /// <summary>
        ///   Executes the action on the UI thread.
        /// </summary>
        /// <param name = "action">The action to execute.</param>
        void OnUIThread(Action action);

        /// <summary>
        /// Executes the handler the fist time the view is loaded.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        void ExecuteOnFirstLoad(object view, Action<object> handler);

        /// <summary>
        /// Executes the handler the next time the view's LayoutUpdated event fires.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        void ExecuteOnLayoutUpdated(object view, Action<object> handler);

        /// <summary>
        /// Get the close action for the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model to close.</param>
        /// <param name="views">The associated views.</param>
        /// <param name="dialogResult">The dialog result.</param>
        /// <returns>An <see cref="Func{T, TResult}"/> to close the view model.</returns>
        Func<CancellationToken, Task> GetViewCloseAction(object viewModel, ICollection<object> views, bool? dialogResult);
    }
}
