using ConsoleContainer.Wpf.Domain;
using ConsoleContainer.Wpf.DragDrop;
using ConsoleContainer.Wpf.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.Controls
{
    internal class DragDropListView : ListView
    {
        private readonly ListViewDragDropManager<SettingsProcessGroupVM> dragMgr;

        public DragDropListView()
        {
            dragMgr = new ListViewDragDropManager<SettingsProcessGroupVM>(this);
        }
    }
}
