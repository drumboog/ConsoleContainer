// Source based on https://github.com/Caliburn-Micro/Caliburn.Micro

namespace ConsoleContainer.Wpf.Eventing
{
    /// <summary>
    ///   Enables easy marshalling of code to the UI thread.
    /// </summary>
    public static class Execute
    {
        /// <summary>
        ///   Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void BeginOnUIThread(this Action action)
        {
            PlatformProvider.Current.BeginOnUIThread(action);
        }

        /// <summary>
        ///   Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name = "action">The action to execute.</param>
        public static Task OnUIThreadAsync(this Func<Task> action)
        {
            return PlatformProvider.Current.OnUIThreadAsync(action);
        }

        /// <summary>
        ///   Executes the action on the UI thread.
        /// </summary>
        /// <param name = "action">The action to execute.</param>
        public static void OnUIThread(this Action action)
        {
            PlatformProvider.Current.OnUIThread(action);
        }
    }
}
