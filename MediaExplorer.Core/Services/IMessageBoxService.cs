using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Services
{
    public enum MessageBoxResult { None, Ok, Cancel, Yes, No };
    public enum MessageBoxImage { Asterisk, Error, Exclamation, Hand, Information, None, Question, Stop, Warning };
    public enum MessageBoxButton { Ok, OkCancel, YesNoCancel, YesNo };

    public interface IMessageBoxService
    {
        Task<MessageBoxResult> ShowAsync(string message);
        Task<MessageBoxResult> ShowAsync(string message, string title);
        Task<MessageBoxResult> ShowAsync(string message, string title, MessageBoxButton button);
        Task<MessageBoxResult> ShowAsync(string message, string title, MessageBoxButton button, MessageBoxImage image);
        Task<MessageBoxResult> ShowAsync(string message, string title, MessageBoxButton button, MessageBoxImage image, MessageBoxResult def);
    }
}
