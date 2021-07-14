using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaExplorer.Core.Services;
using Windows.UI.Xaml.Controls;

namespace MediaExplorer.Uwp.Services
{
    class MessageBoxService : IMessageBoxService
    {
        public async Task<MessageBoxResult> ShowAsync(string message)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageBoxResult> ShowAsync(string message, string title)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageBoxResult> ShowAsync(string message, string title, MessageBoxButton button)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageBoxResult> ShowAsync(string message, string title, MessageBoxButton button, MessageBoxImage image)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageBoxResult> ShowAsync(string message, string title, MessageBoxButton button, MessageBoxImage image, MessageBoxResult def)
        {
            var dialog = new ContentDialog();
            dialog.Content = message;
            dialog.Title = title;
            dialog.PrimaryButtonText = PrimaryButtonText(button);
            if (IsSecondaryButtonVisible(button))
            {
                dialog.SecondaryButtonText = "No";
            }
            if (IsCloseButtonVisible(button))
            {
                dialog.CloseButtonText = "Cancel";
            }

            return ConvertResult(await dialog.ShowAsync(), button);
        }

        private string PrimaryButtonText(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.YesNo:
                case MessageBoxButton.YesNoCancel:
                    return "Yes";
                default:
                    return "Ok";
            }
        }

        private bool IsSecondaryButtonVisible(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.Ok:
                case MessageBoxButton.OkCancel:
                    return false;
                default:
                    return true;
            }
        }

        private bool IsCloseButtonVisible(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.Ok:
                case MessageBoxButton.YesNo:
                    return false;
                default:
                    return true;
            }
        }

        private MessageBoxResult ConvertResult(ContentDialogResult result, MessageBoxButton button)
        {
            switch (result)
            {
                case ContentDialogResult.Primary:
                    switch (button)
                    {
                        case MessageBoxButton.Ok:
                        case MessageBoxButton.OkCancel:
                            return MessageBoxResult.Ok;
                        default:
                            return MessageBoxResult.Yes;
                    }
                case ContentDialogResult.Secondary:
                    return MessageBoxResult.No;
                default:
                    return MessageBoxResult.None;
            }
        }
    }
}
