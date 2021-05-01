using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Services
{
    public enum MessageBoxResult { None, Ok, Cancel, Yes, No };
    public enum MessageBoxImage { Asterisk, Error, Exclamation, Hand, Information, None, Question, Stop, Warning };
    public enum MessageBoxButton { Ok, OkCancel, YesNoCancel, YesNo };

    public interface IMessageBoxService
    {
        MessageBoxResult Show(string message);
        MessageBoxResult Show(string message, string title);
        MessageBoxResult Show(string message, string title, MessageBoxButton button);
        MessageBoxResult Show(string message, string title, MessageBoxButton button, MessageBoxImage image);
        MessageBoxResult Show(string message, string title, MessageBoxButton button, MessageBoxImage image, MessageBoxResult def);
    }
}
