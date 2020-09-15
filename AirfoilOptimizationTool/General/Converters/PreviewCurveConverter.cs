using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace AirfoilOptimizationTool.General.Converters {
    class PreviewCurveConverter : MarkupExtension, System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value as Point[] == null) return null;

            AirfoilPreview.Scaler pointScaler = new AirfoilPreview.Scaler((Size)parameter, value as Point[]);
            return new PointCollection(pointScaler.adjustScale(value as Point[], true));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }
}
