﻿using MvvmCross.Converters;
using System;
using System.Globalization;

namespace MediaExplorer.Core.Converters
{
    public class InverseBooleanConverter : MvxValueConverter<bool, bool>
    {
        protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }
    }
}
