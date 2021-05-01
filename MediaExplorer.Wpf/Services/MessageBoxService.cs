using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using MediaExplorer.Core.Services;

namespace MediaExplorer.Wpf.Services
{
    class MessageBoxService : IMessageBoxService
    {
        public Core.Services.MessageBoxResult Show(string message)
        {
            return ConvertResult(MessageBox.Show(message));
        }
        public Core.Services.MessageBoxResult Show(string message, string title)
        {
            return ConvertResult(MessageBox.Show(message, title));
        }
        public Core.Services.MessageBoxResult Show(string message, string title, Core.Services.MessageBoxButton button)
        {
            return ConvertResult(MessageBox.Show(message, title, ConvertButton(button)));
        }
        public Core.Services.MessageBoxResult Show(string message, string title, Core.Services.MessageBoxButton button, Core.Services.MessageBoxImage image)
        {
            return ConvertResult(MessageBox.Show(message, title, ConvertButton(button), ConvertImage(image)));
        }
        public Core.Services.MessageBoxResult Show(string message, string title, Core.Services.MessageBoxButton button, Core.Services.MessageBoxImage image, Core.Services.MessageBoxResult def)
        {
            return ConvertResult(MessageBox.Show(message, title, ConvertButton(button), ConvertImage(image), ConvertResult(def)));
        }

        private System.Windows.MessageBoxResult ConvertResult(Core.Services.MessageBoxResult result)
        {
            switch(result)
            {
                case Core.Services.MessageBoxResult.Yes:
                    return System.Windows.MessageBoxResult.Yes;
                case Core.Services.MessageBoxResult.No:
                    return System.Windows.MessageBoxResult.No;
                case Core.Services.MessageBoxResult.Cancel:
                    return System.Windows.MessageBoxResult.Cancel;
                case Core.Services.MessageBoxResult.None:
                    return System.Windows.MessageBoxResult.None;
                case Core.Services.MessageBoxResult.Ok:
                    return System.Windows.MessageBoxResult.OK;
                default:
                    return System.Windows.MessageBoxResult.None;
            }
        }

        private Core.Services.MessageBoxResult ConvertResult(System.Windows.MessageBoxResult result)
        {
            switch (result)
            {
                case System.Windows.MessageBoxResult.Yes:
                    return Core.Services.MessageBoxResult.Yes;
                case System.Windows.MessageBoxResult.No:
                    return Core.Services.MessageBoxResult.No;
                case System.Windows.MessageBoxResult.Cancel:
                    return Core.Services.MessageBoxResult.Cancel;
                case System.Windows.MessageBoxResult.None:
                    return Core.Services.MessageBoxResult.None;
                case System.Windows.MessageBoxResult.OK:
                    return Core.Services.MessageBoxResult.Ok;
                default:
                    return Core.Services.MessageBoxResult.None;
            }
        }

        private System.Windows.MessageBoxButton ConvertButton(Core.Services.MessageBoxButton button)
        {
            switch(button)
            {
                case Core.Services.MessageBoxButton.Ok:
                    return System.Windows.MessageBoxButton.OK;
                case Core.Services.MessageBoxButton.OkCancel:
                    return System.Windows.MessageBoxButton.OKCancel;
                case Core.Services.MessageBoxButton.YesNo:
                    return System.Windows.MessageBoxButton.YesNo;
                case Core.Services.MessageBoxButton.YesNoCancel:
                    return System.Windows.MessageBoxButton.YesNoCancel;
                default:
                    return System.Windows.MessageBoxButton.OK;
            }
        }

        private System.Windows.MessageBoxImage ConvertImage(Core.Services.MessageBoxImage image)
        {
            switch(image)
            {
                case Core.Services.MessageBoxImage.Asterisk:
                    return System.Windows.MessageBoxImage.Asterisk;
                case Core.Services.MessageBoxImage.Error:
                    return System.Windows.MessageBoxImage.Error;
                case Core.Services.MessageBoxImage.Exclamation:
                    return System.Windows.MessageBoxImage.Exclamation;
                case Core.Services.MessageBoxImage.Hand:
                    return System.Windows.MessageBoxImage.Hand;
                case Core.Services.MessageBoxImage.Information:
                    return System.Windows.MessageBoxImage.Information;
                case Core.Services.MessageBoxImage.None:
                    return System.Windows.MessageBoxImage.None;
                case Core.Services.MessageBoxImage.Question:
                    return System.Windows.MessageBoxImage.Question;
                case Core.Services.MessageBoxImage.Stop:
                    return System.Windows.MessageBoxImage.Stop;
                case Core.Services.MessageBoxImage.Warning:
                    return System.Windows.MessageBoxImage.Warning;
                default:
                    return System.Windows.MessageBoxImage.None;
            }
        }
    }
}
