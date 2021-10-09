using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

using Nielsen.TechTest.Q1.Common;

namespace Nielsen.TechTest.Q1.RemoteRobotController
{
    public class RectangleSize : BaseObject
    {
        private int _width = 0;
        private int _height = 0;

        public virtual int Width
        {
            get { return this._width; }
            set
            {
                if (this._width != value)
                {
                    this._width = value;
                    this.NotifyPropertyChanged(() => this.Width);
                }
            }
        }

        public virtual int Height
        {
            get { return this._height; }
            set
            {
                if (this._height != value)
                {
                    this._height = value;
                    this.NotifyPropertyChanged(() => this.Height);
                }
            }
        }

    }

    public class LengthHalfCalculator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var intValue = System.Convert.ToInt32(value);
                return intValue / 2;
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var intValue = System.Convert.ToInt32(value);
                return intValue*2;
            }
            catch
            {
                return 0;
            }
        }
    }
}
