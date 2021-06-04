using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MediaExplorer.Wpf.Converters
{
    class ConditionObjectVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Core.Models.Condition.Operation operation = Core.Models.Condition.OperationNameMap[(string)value];
            switch(operation)
            {
                case Core.Models.Condition.Operation.And:
                case Core.Models.Condition.Operation.Or:
                default:
                    return Visibility.Collapsed;
                case Core.Models.Condition.Operation.None:
                case Core.Models.Condition.Operation.Not:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
