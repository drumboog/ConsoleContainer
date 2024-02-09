// Source based on https://github.com/Caliburn-Micro/Caliburn.Micro

using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace ConsoleContainer.Wpf.Eventing
{
    /// <summary>
    /// A <see cref="IPlatformProvider"/> implementation for the XAML platfrom.
    /// </summary>
    public class WindowsPlatformProvider : IPlatformProvider
    {
        private readonly Dispatcher dispatcher;


        /// <summary>
        /// Initializes a new instance of the <see cref="XamlPlatformProvider"/> class.
        /// </summary>
        public WindowsPlatformProvider()
        {
            dispatcher = Application.Current.Dispatcher;
        }

        /// <summary>
        /// Whether or not classes should execute property change notications on the UI thread.
        /// </summary>
        public virtual bool PropertyChangeNotificationsOnUIThread => true;

        private void ValidateDispatcher()
        {
            if (dispatcher == null)
                throw new InvalidOperationException("Not initialized with dispatcher.");
        }

        private bool CheckAccess()
        {
            return dispatcher == null || Application.Current != null;
        }

        /// <summary>
        /// Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public virtual void BeginOnUIThread(System.Action action)
        {
            ValidateDispatcher();
            var dummy = dispatcher.InvokeAsync(() => action());
        }

        /// <summary>
        /// Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns></returns>
        public virtual Task OnUIThreadAsync(Func<Task> action)
        {
            ValidateDispatcher();
            return dispatcher.InvokeAsync(() => action()).Task;

        }

        /// <summary>
        /// Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnUIThread(Action action)
        {
            if (CheckAccess())
                action();
            else
            {
                dispatcher.InvokeAsync(() => action()).Wait();
            }
        }

        private static readonly DependencyProperty PreviouslyAttachedProperty = DependencyProperty.RegisterAttached(
            "PreviouslyAttached",
            typeof(bool),
            typeof(WindowsPlatformProvider),
            null
            );

        /// <summary>
        /// Executes the handler the fist time the view is loaded.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public virtual void ExecuteOnFirstLoad(object view, Action<object> handler)
        {
            //var element = view as FrameworkElement;
            //if (element != null && !(bool) element.GetValue(PreviouslyAttachedProperty)) {
            //    element.SetValue(PreviouslyAttachedProperty, true);
            //    View.ExecuteOnLoad(element, (s, e) => handler(s));
            //}
        }

        /// <summary>
        /// Executes the handler the next time the view's LayoutUpdated event fires.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public virtual void ExecuteOnLayoutUpdated(object view, Action<object> handler)
        {
            //var element = view as FrameworkElement;
            //if (element != null) {
            //    View.ExecuteOnLayoutUpdated(element, (s, e) => handler(s));
            //}
        }

        /// <summary>
        /// Get the close action for the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model to close.</param>
        /// <param name="views">The associated views.</param>
        /// <param name="dialogResult">The dialog result.</param>
        /// <returns>
        /// An <see cref="Action" /> to close the view model.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual Func<CancellationToken, Task> GetViewCloseAction(object viewModel, ICollection<object> views, bool? dialogResult)
        {
            foreach (var contextualView in views)
            {
                var viewType = contextualView.GetType();

                var closeMethod = viewType.GetRuntimeMethod("Close", new Type[0]);

                if (closeMethod != null)
                    return ct => {

                        closeMethod.Invoke(contextualView, null);
                        return Task.FromResult(true);
                    };

                var isOpenProperty = viewType.GetRuntimeProperty("IsOpen");

                if (isOpenProperty != null)
                {
                    return ct =>
                    {
                        isOpenProperty.SetValue(contextualView, false, null);

                        return Task.FromResult(true);
                    };
                }
            }

            return ct =>
            {
                return Task.FromResult(true);
            };
        }
    }
}
