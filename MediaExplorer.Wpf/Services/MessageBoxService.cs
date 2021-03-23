using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using MediaExplorer.Core.Services;

namespace MediaExplorer.Wpf.Services
{
    class MessageBoxService : IMessageBoxService
    {
        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }
    }
}
