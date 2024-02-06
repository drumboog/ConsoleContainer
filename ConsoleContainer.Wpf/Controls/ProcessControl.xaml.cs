﻿using ConsoleContainer.Eventing;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ProcessControl.xaml
    /// </summary>
    public partial class ProcessControl : UserControl
    {
        public static DependencyProperty ProcessProperty = DependencyProperty.Register("Process", typeof(ProcessVM), typeof(ProcessControl));
        private IEventAggregator eventAggregator = App.EventAggregator;

        internal ProcessVM Process
        {
            get { return (ProcessVM)GetValue(ProcessProperty); }
            set { SetValue(ProcessProperty, value); }
        }

        public ProcessControl()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Process.StartProcess();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Process.StopProcess();
        }

        private void ClearOutput_Click(object sender, RoutedEventArgs e)
        {
            Process.ClearOutput();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            _ = eventAggregator.PublishOnCurrentThreadAsync(new EditProcessEvent(Process));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _ = eventAggregator.PublishOnCurrentThreadAsync(new DeleteProcessEvent(Process));
        }
    }
}
