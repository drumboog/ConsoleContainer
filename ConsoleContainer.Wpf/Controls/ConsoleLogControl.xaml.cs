﻿using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ConsoleContainer.Wpf.Controls
{
    internal class ConsoleLogVM : ViewModel
    {
        public bool AutoScroll
        {
            get => GetProperty(true);
            set => SetProperty(value);
        }
    }

    /// <summary>
    /// Interaction logic for ConsoleLog.xaml
    /// </summary>
    public partial class ConsoleLogControl : UserControl
    {
        private const int BUFFER_SIZE = 500_000;

        private ConsoleLogVM viewModel = new ConsoleLogVM();
        private int currentLength = 0;

        public ConsoleLogControl()
        {
            InitializeComponent();
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            DataContext = viewModel;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.AutoScroll) &&
                viewModel.AutoScroll)
            {
                rtbLog.ScrollToEnd();
            }
        }

        public void AddOutput(string? message, Brush? foreground = null)
        {
            Dispatcher.Invoke(() => AddMessage(message, foreground));
        }

        public void ClearLogs()
        {
            Dispatcher.Invoke(() =>
            {
                TextRange txt = new TextRange(rtbFlowDoc.ContentStart, rtbFlowDoc.ContentEnd);
                txt.Text = "";
                currentLength = 0;
            });
        }

        private void AddMessage(string? message, Brush? foreground = null)
        {
            var run = new Run(message);
            run.Foreground = foreground ?? Brushes.White;
            var paragraph = new Paragraph(run);
            rtbFlowDoc.Blocks.Add(paragraph);
            currentLength += message?.Length ?? 1;
            while (currentLength > BUFFER_SIZE)
            {
                var firstBlock = rtbFlowDoc.Blocks.FirstOrDefault();
                if (firstBlock is null)
                {
                    break;
                }
                TextRange content = new TextRange(firstBlock.ContentStart, firstBlock.ContentEnd);
                currentLength -= content.Text.Length;
                rtbFlowDoc.Blocks.Remove(firstBlock);
            }
            if (viewModel.AutoScroll)
            {
                rtbLog.ScrollToEnd();
            }
        }
    }
}
